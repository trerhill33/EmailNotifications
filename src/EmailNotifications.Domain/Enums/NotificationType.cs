namespace EmailNotifications.Domain.Enums;

/// <summary>
/// Represents the type of notification being sent
/// </summary>
public enum NotificationType
{
    /// <summary>
    /// Notification sent when a new user is created
    /// </summary>
    NewUser = 1,

    /// <summary>
    /// Notification sent when a user requests a password reset
    /// </summary>
    PasswordReset = 2,

    /// <summary>
    /// Welcome email sent to new users
    /// </summary>
    Welcome = 3
} 