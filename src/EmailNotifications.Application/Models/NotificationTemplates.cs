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
        return new NotificationRequest<CreateUserTemplateModel>(NotificationType.UserCreated, model);
    }

    public static NotificationRequest<ResetPasswordTemplateModel> PasswordReset(string firstName, string oneTimePassword, string expiryTimeFormatted)
    {
        var model = new ResetPasswordTemplateModel(firstName, oneTimePassword, expiryTimeFormatted);
        return new NotificationRequest<ResetPasswordTemplateModel>(NotificationType.PasswordReset, model);
    }

    public static NotificationRequest<PasswordChangedTemplateModel> PasswordChanged(string firstName, DateTime changeDate)
    {
        var model = new PasswordChangedTemplateModel(firstName, changeDate);
        return new NotificationRequest<PasswordChangedTemplateModel>(NotificationType.PasswordChanged, model);
    }
}