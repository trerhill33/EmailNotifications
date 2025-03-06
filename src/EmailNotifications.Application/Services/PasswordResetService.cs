using EmailNotifications.Application.Common.Notifications.Interfaces;
using EmailNotifications.Application.Common.Notifications.Models;
using Microsoft.Extensions.Logging;

namespace EmailNotifications.Application.Services;

public sealed class PasswordResetService(
    INotificationService notificationService,
    ILogger<PasswordResetService> logger)
{
    private readonly INotificationService _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
    private readonly ILogger<PasswordResetService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<bool> SendPasswordResetNotificationAsync(
        string firstName,
        string lastName,
        string oneTimePassword,
        int expiryHours = 24,
        CancellationToken cancellationToken = default)
    {
        try
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(firstName);
            ArgumentException.ThrowIfNullOrWhiteSpace(lastName);
            ArgumentException.ThrowIfNullOrWhiteSpace(oneTimePassword);

            _logger.LogInformation("Sending password reset notification to {FirstName} {LastName}", firstName, lastName);

            var expiryTime = DateTime.UtcNow.AddHours(expiryHours);

            var result = await _notificationService.SendAsync(NotificationTemplates.PasswordReset(
                firstName,
                oneTimePassword,
                expiryTime.ToString("f")
            ), cancellationToken);

            if (result)
            {
                _logger.LogInformation("Successfully sent password reset notification to {FirstName} {LastName}", firstName, lastName);
            }
            else
            {
                _logger.LogWarning("Failed to send password reset notification to {FirstName} {LastName}", firstName, lastName);
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending password reset notification to {FirstName} {LastName}", firstName, lastName);
            return false;
        }
    }
}