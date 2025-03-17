using EmailNotifications.Infrastructure.Models;

namespace EmailNotifications.Infrastructure.Interfaces;

/// <summary>
/// Service for sending emails
/// </summary>
public interface IEmailSender
{
    /// <summary>
    /// Sends an email with built-in retry logic for transient errors
    /// </summary>
    Task SendEmailAsync(EmailMessage emailMessage, CancellationToken cancellationToken = default);
}