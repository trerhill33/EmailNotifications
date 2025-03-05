using System.Net.Mail;
using System.Security.Cryptography.X509Certificates;
using EmailNotifications.Infrastructure.Configuration;
using EmailNotifications.Infrastructure.Exceptions;
using EmailNotifications.Infrastructure.Interfaces;
using EmailNotifications.Infrastructure.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EmailNotifications.Infrastructure.Services;

public class SmtpEmailSender : IEmailSender
{
    private readonly SmtpClient _smtpClient;
    private readonly ILogger<SmtpEmailSender> _logger;
    private readonly MailRelaySettings _settings;

    public SmtpEmailSender(
        IOptions<MailRelaySettings> settings,
        ILogger<SmtpEmailSender> logger)
    {
        _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        try
        {
            // Configure the SMTP client with the mail relay settings
            _smtpClient = new SmtpClient(_settings.Server, _settings.Port)
            {
                EnableSsl = _settings.UseSsl
            };

            // If an intermediate certificate is provided, load it
            if (string.IsNullOrEmpty(_settings.IntermediateCertificatePath)) return;
            
            try
            {
                var certificate = new X509Certificate2(_settings.IntermediateCertificatePath);
                _smtpClient.ClientCertificates.Add(certificate);
                _logger.LogInformation(
                    "Loaded intermediate certificate from {CertificatePath} for SMTP server {Server}:{Port}",
                    _settings.IntermediateCertificatePath,
                    _settings.Server,
                    _settings.Port);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to load intermediate certificate from {CertificatePath} for SMTP server {Server}:{Port}",
                    _settings.IntermediateCertificatePath,
                    _settings.Server,
                    _settings.Port);
                throw new EmailConfigurationException(
                    $"Failed to load intermediate certificate from {_settings.IntermediateCertificatePath}", 
                    ex);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to configure SMTP client for server {Server}:{Port}",
                _settings.Server,
                _settings.Port);
            throw new EmailConfigurationException("Failed to configure SMTP client", ex);
        }
    }
    public async Task SendEmailAsync(EmailMessage emailMessage, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(emailMessage);

        var correlationId = Guid.NewGuid().ToString();
        var recipients = string.Join(", ", emailMessage.To.Select(t => t.Address));
        var attempts = 0;
        var retryDelayMs = _settings.RetryDelayMilliseconds;

        _logger.LogInformation(
            "Starting email send operation {CorrelationId} to {Recipients}. Max attempts: {MaxAttempts}",
            correlationId,
            recipients,
            _settings.MaxRetryAttempts);

        using var mailMessage = CreateMailMessage(emailMessage);

        while (attempts < _settings.MaxRetryAttempts)
        {
            try
            {
                attempts++;
                _logger.LogDebug(
                    "Attempt {Attempt} of {MaxAttempts} to send email {CorrelationId} via SMTP server {Server}:{Port}",
                    attempts,
                    _settings.MaxRetryAttempts,
                    correlationId,
                    _settings.Server,
                    _settings.Port);

                await _smtpClient.SendMailAsync(mailMessage, cancellationToken);

                _logger.LogInformation(
                    "Email {CorrelationId} sent successfully to {Recipients} on attempt {Attempt}",
                    correlationId,
                    recipients,
                    attempts);
                
                return;
            }
            catch (SmtpException ex) when (IsTransientError(ex) && attempts < _settings.MaxRetryAttempts)
            {
                _logger.LogWarning(
                    ex,
                    "Transient SMTP error on attempt {Attempt} of {MaxAttempts} for email {CorrelationId}. " +
                    "Retrying in {RetryDelay}ms. Status code: {StatusCode}",
                    attempts,
                    _settings.MaxRetryAttempts,
                    correlationId,
                    retryDelayMs,
                    ex.StatusCode);

                await Task.Delay(retryDelayMs, cancellationToken);

                // Increase delay for next retry (exponential backoff)
                retryDelayMs *= 2;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Failed to send email {CorrelationId} to {Recipients}. Subject: {Subject}. Error: {ErrorMessage}",
                    correlationId,
                    recipients,
                    emailMessage.Subject,
                    ex.Message);
                throw new EmailSendException($"Failed to send email to {recipients}", ex);
            }
        }

        var exception = new EmailSendException($"Failed to send email to {recipients} after {_settings.MaxRetryAttempts} attempts");
        _logger.LogError(
            exception,
            "Failed to send email {CorrelationId} after {MaxAttempts} attempts",
            correlationId,
            _settings.MaxRetryAttempts);
        throw exception;
    }

    private static MailMessage CreateMailMessage(EmailMessage emailMessage)
    {
        var mailMessage = new MailMessage
        {
            Subject = emailMessage.Subject,
            Body = emailMessage.HtmlBody,
            IsBodyHtml = true,
            From = emailMessage.From,
            Priority = (MailPriority)emailMessage.Priority
        };

        if (emailMessage.TextBody is not null)
        {
            // Add plain text alternative view (Better support)
            var plainView = AlternateView.CreateAlternateViewFromString(
                emailMessage.TextBody,
                null,
                "text/plain");

            mailMessage.AlternateViews.Add(plainView);

            // Add HTML view
            var htmlView = AlternateView.CreateAlternateViewFromString(
                emailMessage.HtmlBody,
                null,
                "text/html");

            mailMessage.AlternateViews.Add(htmlView);
        }

        if (emailMessage.ReplyTo is not null)
        {
            mailMessage.ReplyToList.Add(emailMessage.ReplyTo);
        }

        foreach (var recipient in emailMessage.To)
        {
            mailMessage.To.Add(recipient);
        }

        foreach (var recipient in emailMessage.Cc)
        {
            mailMessage.CC.Add(recipient);
        }

        foreach (var recipient in emailMessage.Bcc)
        {
            mailMessage.Bcc.Add(recipient);
        }

        // Add attachments if any
        foreach (var attachment in emailMessage.Attachments)
        {
            try
            {
                var stream = new MemoryStream(attachment.Content.ToArray());
                mailMessage.Attachments.Add(new Attachment(stream, attachment.FileName, attachment.ContentType));
            }
            catch (Exception ex)
            {
                throw new EmailAttachmentException($"Failed to add attachment {attachment.FileName}", ex);
            }
        }

        return mailMessage;
    }

    private static bool IsTransientError(SmtpException exception) =>
        // Consider certain SMTP status codes as transient errors
        // See: https://datatracker.ietf.org/doc/html/rfc5321#section-4.2.1
        exception.StatusCode switch
        {
            SmtpStatusCode.ServiceNotAvailable => true,
            SmtpStatusCode.MailboxBusy => true,
            SmtpStatusCode.MailboxUnavailable => true,
            SmtpStatusCode.TransactionFailed => true,
            SmtpStatusCode.GeneralFailure => true,
            _ => false
        };
}