namespace EmailNotifications.Application.Enums;

/// <summary>
/// Represents the type of notification to be sent
/// </summary>
public enum NotificationType
{
    UserCreated = 1,
    PasswordReset = 2,
    PasswordChanged = 3
}