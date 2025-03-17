using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using EmailNotifications.Infrastructure.Configuration;
using EmailNotifications.Infrastructure.Exceptions;
using EmailNotifications.Infrastructure.Helper;
using EmailNotifications.Infrastructure.Interfaces;
using EmailNotifications.Infrastructure.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EmailNotifications.Infrastructure.Services;

public class SmtpEmailSender(
    IOptions<MailRelaySettings> settings,
    ICertHelper certHelper,
    ILogger<SmtpEmailSender> logger)
    : IEmailSender
{
    private readonly ILogger<SmtpEmailSender> _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    private readonly MailRelaySettings _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
    private readonly ICertHelper _certHelper = certHelper ?? throw new ArgumentNullException(nameof(certHelper));

    public async Task SendEmailAsync(EmailMessage emailMessage, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(emailMessage);

        var correlationId = Guid.NewGuid().ToString();
        var recipients = string.Join(", ", emailMessage.To.Select(t => t.Address));

        _logger.LogInformation("Starting email send operation {CorrelationId} to {Recipients}. Max attempts: {MaxAttempts}",
            correlationId, recipients, _settings.MaxRetryAttempts);

        using var smtpClient = CreateSmtpClient();
        using var mailMessage = CreateMailMessage(emailMessage);

        await SendWithRetryAsync(smtpClient, mailMessage, correlationId, recipients, cancellationToken);
    }

    private SmtpClient CreateSmtpClient()
    {
        var smtpClient = new SmtpClient(_settings.Server, _settings.Port)
        {
            EnableSsl = _settings.UseSsl,
            Timeout = _settings.Timeout
        };

        if (_settings.UseCustomServerCertificateValidation &&
            !string.IsNullOrEmpty(_settings.ServerIntermediateCertificateSecret))
        {
            try
            {
                var intermediateCerts = _certHelper.GetIntermediateCertificatesAsync(_settings.ServerIntermediateCertificateSecret);

                ServicePointManager.ServerCertificateValidationCallback = (sender, cert, chain, sslPolicyErrors) =>
                {
                    _logger.LogDebug("Validating server certificate for {Server}:{Port}. SSL policy errors: {SslPolicyErrors}",
                        _settings.Server, _settings.Port, sslPolicyErrors);

                    chain.ChainPolicy.ExtraStore.AddRange(intermediateCerts);

                    var isValid = chain.Build((X509Certificate2)cert);

                    if (!isValid)
                    {
                        _logger.LogWarning("Server certificate chain validation failed for {Server}:{Port}. Chain status: {ChainStatus}",
                            _settings.Server, _settings.Port,
                            string.Join(", ", chain.ChainStatus.Select(s => s.Status)));
                    }
                    else
                    {
                        _logger.LogDebug("Server certificate chain validation succeeded for {Server}:{Port}",
                            _settings.Server, _settings.Port);
                    }

                    return isValid;
                };

                _logger.LogInformation("Configured custom server certificate validation for {Server}:{Port}", _settings.Server, _settings.Port);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to configure server certificate validation for {Server}:{Port}", _settings.Server, _settings.Port);
                throw new EmailConfigurationException("Failed to configure server certificate validation", ex);
            }
        }

        return smtpClient;
    }
    
    private static MailMessage CreateMailMessage(EmailMessage emailMessage)
    {
        var mailMessage = new MailMessage
        {
            Subject = emailMessage.Subject,
            From = emailMessage.From,
            Priority = (MailPriority)emailMessage.Priority
        };

        // Set HTML body as an AlternateView with UTF-8 encoding
        var htmlView = AlternateView.CreateAlternateViewFromString(
            emailMessage.HtmlBody,
            Encoding.UTF8, // Use Encoding instead of ContentType
            "text/html");
        htmlView.TransferEncoding = TransferEncoding.SevenBit; // Avoid Quoted-Printable encoding
        mailMessage.AlternateViews.Add(htmlView);

        if (emailMessage.TextBody is not null)
        {
            var plainView = AlternateView.CreateAlternateViewFromString(
                emailMessage.TextBody,
                Encoding.UTF8,
                "text/plain");
            plainView.TransferEncoding = TransferEncoding.SevenBit; // Avoid Quoted-Printable encoding
            mailMessage.AlternateViews.Add(plainView);
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

    private async Task SendWithRetryAsync(SmtpClient smtpClient, MailMessage mailMessage, string correlationId, string recipients, CancellationToken cancellationToken)
    {
        var attempts = 0;
        var retryDelayMs = _settings.RetryDelayMilliseconds;

        while (attempts < _settings.MaxRetryAttempts)
        {
            try
            {
                attempts++;
                _logger.LogDebug("Attempt {Attempt} of {MaxAttempts} to send email {CorrelationId} via SMTP server {Server}:{Port}",
                    attempts, _settings.MaxRetryAttempts, correlationId, _settings.Server, _settings.Port);

                await smtpClient.SendMailAsync(mailMessage, cancellationToken);

                _logger.LogInformation("Email {CorrelationId} sent successfully to {Recipients} on attempt {Attempt}",
                    correlationId, recipients, attempts);
                return;
            }
            catch (SmtpException ex) when (IsTransientError(ex) && attempts < _settings.MaxRetryAttempts)
            {
                _logger.LogWarning(ex, "Transient SMTP error on attempt {Attempt} of {MaxAttempts} for email {CorrelationId}. Retrying in {RetryDelay}ms. Status code: {StatusCode}",
                    attempts, _settings.MaxRetryAttempts, correlationId, retryDelayMs, ex.StatusCode);

                await Task.Delay(retryDelayMs, cancellationToken);
                retryDelayMs *= 2; // Exponential backoff
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email {CorrelationId} to {Recipients}. Subject: {Subject}. Error: {ErrorMessage}",
                    correlationId, recipients, mailMessage.Subject, ex.Message);
                throw new EmailSendException($"Failed to send email to {recipients}", ex);
            }
        }

        var exception = new EmailSendException($"Failed to send email to {recipients} after {_settings.MaxRetryAttempts} attempts");
        _logger.LogError(exception, "Failed to send email {CorrelationId} after {MaxAttempts} attempts",
            correlationId, _settings.MaxRetryAttempts);
        throw exception;
    }

    private static bool IsTransientError(SmtpException exception) =>
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