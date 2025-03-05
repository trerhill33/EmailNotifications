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
    /// <param name="emailMessage">The email message to send</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task SendEmailAsync(EmailMessage emailMessage, CancellationToken cancellationToken = default);
}