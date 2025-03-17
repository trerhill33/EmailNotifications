using EmailNotifications.Application.Common.Notifications.Interfaces;
using EmailNotifications.Domain.Enums;

namespace EmailNotifications.Application.Common.Notifications.Models;

/// <summary>
/// Factory Methods
/// Provides type-safe methods for creating notification requests with their corresponding template models
/// </summary>
public static class NotificationFactory
{
    // User Notifications
    public static SendNotificationRequest<CreateUserTemplateModel> UserCreated(
        string firstName, 
        string lastName, 
        string formattedDate)
    {
        var model = new CreateUserTemplateModel(firstName, lastName, formattedDate);
        return new SendNotificationRequest<CreateUserTemplateModel>(NotificationType.NewUser, model);
    }

    public static SendNotificationRequest<ResetPasswordTemplateModel> PasswordReset(
        string firstName, 
        string oneTimePassword,
        string expiryTimeFormatted)
    {
        var model = new ResetPasswordTemplateModel(firstName, oneTimePassword, expiryTimeFormatted);
        return new SendNotificationRequest<ResetPasswordTemplateModel>(NotificationType.PasswordReset, model);
    }

    // Weekly Report Notifications
    public static SendNotificationRequest<WeeklySummary> WeeklySummary(
        string reportTitle,
        int totalShipments,
        decimal totalCost,
        IReadOnlyCollection<IAttachment> attachments)
    {
        var model = new WeeklySummary(reportTitle, totalCost);
        return new SendNotificationRequest<WeeklySummary>(NotificationType.WeeklySummary, model, attachments);
    }

    public static SendNotificationRequest<PendingApprovalsModel> PendingApproval(
        string approverName,
        int pendingCount,
        IReadOnlyCollection<IAttachment> attachments)
    {
        var model = new PendingApprovalsModel(approverName, pendingCount);
        return new SendNotificationRequest<PendingApprovalsModel>(NotificationType.PendingApprovals, model, attachments);
    }
}