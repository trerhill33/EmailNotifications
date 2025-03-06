using EmailNotifications.Application.Common.Notifications.Interfaces;
using EmailNotifications.Application.Common.Notifications.Models;
using Microsoft.AspNetCore.Mvc;

namespace EmailNotifications.API.Controllers;

/// <summary>
/// Controller for sending email notifications
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class NotificationsController : ControllerBase
{
    private readonly INotificationService _notificationService;
    private readonly ILogger<NotificationsController> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationsController"/> class.
    /// </summary>
    /// <param name="notificationService">The notification service.</param>
    /// <param name="logger">The logger.</param>
    public NotificationsController(
        INotificationService notificationService,
        ILogger<NotificationsController> logger)
    {
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Sends a new user created notification
    /// </summary>
    /// <param name="request">The request containing user information</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>An action result indicating success or failure</returns>
    [HttpPost("new-user")]
    public async Task<IActionResult> SendNewUserNotification(
        [FromBody] NewUserNotificationRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Sending new user notification for user {FirstName} {LastName}",
                request.FirstName, request.LastName);

            var notificationRequest = NotificationTemplates.UserCreated(
                request.FirstName,
                request.LastName,
                DateTime.UtcNow.ToString("f")
            );

            bool success = await _notificationService.SendAsync(notificationRequest, cancellationToken);

            if (success)
            {
                _logger.LogInformation("Successfully sent new user notification to {FirstName} {LastName}",
                    request.FirstName, request.LastName);
                return Ok(new { message = "Notification sent successfully" });
            }

            _logger.LogWarning("Failed to send new user notification to {FirstName} {LastName}",
                request.FirstName, request.LastName);
            return StatusCode(500, new { message = "Failed to send notification" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending new user notification to {FirstName} {LastName}",
                request.FirstName, request.LastName);
            return StatusCode(500, new { message = "An error occurred while sending the notification" });
        }
    }

    /// <summary>
    /// Sends a password reset notification
    /// </summary>
    /// <param name="request">The request containing user information</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>An action result indicating success or failure</returns>
    [HttpPost("password-reset")]
    public async Task<IActionResult> SendPasswordResetNotification(
        [FromBody] PasswordResetNotificationRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Sending password reset notification for user {FirstName}", request.FirstName);

            var notificationRequest = NotificationTemplates.PasswordReset(
                request.FirstName,
                request.OneTimePassword,
                DateTime.UtcNow.AddHours(request.ExpiryHours).ToString("f")
            );

            bool success = await _notificationService.SendAsync(notificationRequest, cancellationToken);

            if (success)
            {
                _logger.LogInformation("Successfully sent password reset notification to {FirstName}", request.FirstName);
                return Ok(new { message = "Notification sent successfully" });
            }

            _logger.LogWarning("Failed to send password reset notification to {FirstName}", request.FirstName);
            return StatusCode(500, new { message = "Failed to send notification" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending password reset notification to {FirstName}", request.FirstName);
            return StatusCode(500, new { message = "An error occurred while sending the notification" });
        }
    }
}

/// <summary>
/// Request model for new user notification
/// </summary>
public class NewUserNotificationRequest
{
    /// <summary>
    /// Gets or sets the user's first name
    /// </summary>
    public required string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the user's last name
    /// </summary>
    public required string LastName { get; set; }
}

/// <summary>
/// Request model for password reset notification
/// </summary>
public class PasswordResetNotificationRequest
{
    /// <summary>
    /// Gets or sets the user's first name
    /// </summary>
    public required string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the one-time password
    /// </summary>
    public required string OneTimePassword { get; set; }

    /// <summary>
    /// Gets or sets the number of hours until the password expires
    /// </summary>
    public int ExpiryHours { get; set; } = 24;
}