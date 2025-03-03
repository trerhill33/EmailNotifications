using EmailNotifications.Application.Models;
using EmailNotifications.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Mail;

namespace EmailNotifications.Infrastructure.Services;

/// <summary>
/// SMTP implementation of the email sender
/// </summary>
public class SmtpEmailSender(SmtpClient smtpClient, ILogger<SmtpEmailSender> logger) : IEmailSender
{
    private readonly SmtpClient _smtpClient = smtpClient ?? throw new ArgumentNullException(nameof(smtpClient));
    private readonly ILogger<SmtpEmailSender> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <inheritdoc/>
    public async Task SendEmailAsync(EmailMessage emailMessage, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(emailMessage);

        using var mailMessage = CreateMailMessage(emailMessage);

        try
        {
            await _smtpClient.SendMailAsync(mailMessage, cancellationToken);
            _logger.LogInformation("Email sent successfully to {Recipients}", string.Join(", ", emailMessage.To.Select(t => t.Address)));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Recipients}", string.Join(", ", emailMessage.To.Select(t => t.Address)));
            throw;
        }
    }

    /// <inheritdoc/>
    public async Task<bool> SendEmailWithRetryAsync(
        EmailMessage emailMessage,
        int maxRetryAttempts = 3,
        int retryDelayMilliseconds = 1000,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(emailMessage);

        var attempts = 0;

        while (attempts < maxRetryAttempts)
        {
            try
            {
                attempts++;
                await SendEmailAsync(emailMessage, cancellationToken);
                return true;
            }
            catch (SmtpException ex) when (IsTransientError(ex) && attempts < maxRetryAttempts)
            {
                _logger.LogWarning(ex, "Transient SMTP error on attempt {Attempt} of {MaxAttempts}. Retrying in {RetryDelay}ms...",
                    attempts, maxRetryAttempts, retryDelayMilliseconds);

                await Task.Delay(retryDelayMilliseconds, cancellationToken);

                // Increase delay for next retry (exponential backoff)
                retryDelayMilliseconds *= 2;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Non-transient error occurred while sending email on attempt {Attempt} of {MaxAttempts}",
                    attempts, maxRetryAttempts);
                return false;
            }
        }

        return false;
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
            // Add plain text alternative view
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

        return mailMessage;
    }

    private static bool IsTransientError(SmtpException exception)
    {
        // Consider certain SMTP status codes as transient errors
        // See: https://datatracker.ietf.org/doc/html/rfc5321#section-4.2.1
        return exception.StatusCode switch
        {
            SmtpStatusCode.ServiceNotAvailable => true,
            SmtpStatusCode.MailboxBusy => true,
            SmtpStatusCode.MailboxUnavailable => true,
            SmtpStatusCode.TransactionFailed => true,
            SmtpStatusCode.GeneralFailure => true,
            _ => false
        };
    }
}