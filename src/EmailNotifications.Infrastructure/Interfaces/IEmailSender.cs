using EmailNotifications.Application.Models;

namespace EmailNotifications.Infrastructure.Interfaces;

/// <summary>
/// Service for sending emails
/// </summary>
public interface IEmailSender
{
    /// <summary>
    /// Sends an email
    /// </summary>
    /// <param name="emailMessage">The email message to send</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>A task representing the asynchronous operation</returns>
    Task SendEmailAsync(EmailMessage emailMessage, CancellationToken cancellationToken = default);

    /// <summary>
    /// Sends an email with retry logic for transient errors
    /// </summary>
    /// <param name="emailMessage">The email message to send</param>
    /// <param name="maxRetryAttempts">The maximum number of retry attempts</param>
    /// <param name="retryDelayMilliseconds">The delay between retry attempts in milliseconds</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>True if the email was sent successfully, otherwise false</returns>
    Task<bool> SendEmailWithRetryAsync(
        EmailMessage emailMessage,
        int maxRetryAttempts = 3,
        int retryDelayMilliseconds = 1000,
        CancellationToken cancellationToken = default);
}