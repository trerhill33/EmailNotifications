using EmailNotifications.Application.Enums;

namespace EmailNotifications.Application.Models;

/// <summary>
/// Factory Methods
/// Provides type-safe methods for creating notification requests with their corresponding template models
/// </summary>
public static class NotificationTemplates
{
    public static NotificationRequest<CreateUserTemplateModel> UserCreated(string firstName, string lastName, string formattedDate)
    {
        var model = new CreateUserTemplateModel(firstName, lastName, formattedDate);
        return new NotificationRequest<CreateUserTemplateModel>(NotificationType.NewUser, model);
    }

    public static NotificationRequest<ResetPasswordTemplateModel> PasswordReset(string firstName, string oneTimePassword, string expiryTimeFormatted)
    {
        var model = new ResetPasswordTemplateModel(firstName, oneTimePassword, expiryTimeFormatted);
        return new NotificationRequest<ResetPasswordTemplateModel>(NotificationType.PasswordReset, model);
    }

    public static NotificationRequest<WelcomeTemplateModel> Welcome(string firstName)
    {
        var model = new WelcomeTemplateModel(firstName);
        return new NotificationRequest<WelcomeTemplateModel>(NotificationType.Welcome, model);
    }
}