using EmailNotifications.Application.Common.Notifications.Interfaces;
using EmailNotifications.Domain.Enums;

namespace EmailNotifications.Application.Common.Notifications.Models;

/// <summary>
/// Factory Methods
/// Provides type-safe methods for creating notification requests with their corresponding template models
/// </summary>
public static class NotificationTemplates
{
    // User Notifications
    public static NotificationRequest<CreateUserTemplateModel> UserCreated(
        string firstName, 
        string lastName, 
        string formattedDate,
        IReadOnlyCollection<IAttachment>? attachments = null)
    {
        var model = new CreateUserTemplateModel(firstName, lastName, formattedDate);
        return new NotificationRequest<CreateUserTemplateModel>(NotificationType.NewUser, model, attachments);
    }

    public static NotificationRequest<ResetPasswordTemplateModel> PasswordReset(
        string firstName, 
        string oneTimePassword,
        string expiryTimeFormatted,
        IReadOnlyCollection<IAttachment>? attachments = null)
    {
        var model = new ResetPasswordTemplateModel(firstName, oneTimePassword, expiryTimeFormatted);
        return new NotificationRequest<ResetPasswordTemplateModel>(NotificationType.PasswordReset, model, attachments);
    }

    // Weekly Report Notifications
    public static NotificationRequest<FedExWeeklyChargesSummaryModel> FedExWeeklyChargesSummary(
        string reportTitle,
        string dateRange,
        int totalShipments,
        decimal totalCost,
        IReadOnlyCollection<IAttachment>? attachments = null)
    {
        var model = new FedExWeeklyChargesSummaryModel(reportTitle, dateRange, totalShipments, totalCost);
        return new NotificationRequest<FedExWeeklyChargesSummaryModel>(NotificationType.FedExWeeklyChargesSummary, model, attachments);
    }

    public static NotificationRequest<FedExWeeklyDetailChargesSummaryModel> FedExWeeklyDetailChargesSummary(
        string reportTitle,
        string dateRange,
        int totalShipments,
        decimal totalCost,
        IReadOnlyCollection<IAttachment>? attachments = null)
    {
        var model = new FedExWeeklyDetailChargesSummaryModel(reportTitle, dateRange, totalShipments, totalCost);
        return new NotificationRequest<FedExWeeklyDetailChargesSummaryModel>(NotificationType.FedExWeeklyDetailChargesSummary, model, attachments);
    }

    public static NotificationRequest<FedExFileReceiptModel> FedExFileReceipt(
        string fileName,
        string receivedDate,
        string processedDate,
        IReadOnlyCollection<IAttachment>? attachments = null)
    {
        var model = new FedExFileReceiptModel(fileName, receivedDate, processedDate);
        return new NotificationRequest<FedExFileReceiptModel>(NotificationType.FedExFileReceipt, model, attachments);
    }

    public static NotificationRequest<TrackingNumbersByBusinessUnitModel> TrackingNumbersByBusinessUnit(
        string reportTitle,
        string dateRange,
        int totalBusinessUnits,
        IReadOnlyCollection<IAttachment>? attachments = null)
    {
        var model = new TrackingNumbersByBusinessUnitModel(reportTitle, dateRange, totalBusinessUnits);
        return new NotificationRequest<TrackingNumbersByBusinessUnitModel>(NotificationType.WeeklyTrackingByBusinessUnit, model, attachments);
    }

    public static NotificationRequest<InvalidEmployeeIdModel> InvalidEmployeeId(
        string reportTitle,
        string reportDate,
        int totalInvalidIds,
        IReadOnlyCollection<IAttachment>? attachments = null)
    {
        var model = new InvalidEmployeeIdModel(reportTitle, reportDate, totalInvalidIds);
        return new NotificationRequest<InvalidEmployeeIdModel>(NotificationType.InvalidEmployeeIdSummary, model, attachments);
    }

    // Daily Report Notifications
    public static NotificationRequest<FedExRemittanceSummaryModel> FedExRemittanceSummary(
        string reportTitle,
        string dateRange,
        decimal totalRemittance,
        IReadOnlyCollection<IAttachment>? attachments = null)
    {
        var model = new FedExRemittanceSummaryModel(reportTitle, dateRange, totalRemittance);
        return new NotificationRequest<FedExRemittanceSummaryModel>(NotificationType.FedExRemittanceSummary, model, attachments);
    }

    public static NotificationRequest<FedExRemittanceDetailsModel> FedExRemittanceDetails(
        string reportTitle,
        string dateRange,
        decimal totalRemittance,
        IReadOnlyCollection<IAttachment>? attachments = null)
    {
        var model = new FedExRemittanceDetailsModel(reportTitle, dateRange, totalRemittance);
        return new NotificationRequest<FedExRemittanceDetailsModel>(NotificationType.FedExRemittanceDetails, model, attachments);
    }

    public static NotificationRequest<FedExFileMissingModel> FedExFileMissing(
        string expectedDate,
        string fileType,
        IReadOnlyCollection<IAttachment>? attachments = null)
    {
        var model = new FedExFileMissingModel(expectedDate, fileType);
        return new NotificationRequest<FedExFileMissingModel>(NotificationType.FedExFileMissing, model, attachments);
    }

    public static NotificationRequest<ReassignedTrackingNumbersModel> ReassignedTrackingNumbers(
        string reportTitle,
        string reportDate,
        int totalReassigned,
        IReadOnlyCollection<IAttachment>? attachments = null)
    {
        var model = new ReassignedTrackingNumbersModel(reportTitle, reportDate, totalReassigned);
        return new NotificationRequest<ReassignedTrackingNumbersModel>(NotificationType.DailyReassignedTrackingNumbers, model, attachments);
    }

    public static NotificationRequest<DelayedInvoicesModel> DelayedInvoices(
        string reportTitle,
        string reportDate,
        int totalDelayed,
        IReadOnlyCollection<IAttachment>? attachments = null)
    {
        var model = new DelayedInvoicesModel(reportTitle, reportDate, totalDelayed);
        return new NotificationRequest<DelayedInvoicesModel>(NotificationType.DelayedInvoicesReport, model, attachments);
    }

    public static NotificationRequest<PendingApprovalNotificationModel> PendingApproval(
        string approverName,
        int pendingCount,
        IReadOnlyCollection<IAttachment>? attachments = null)
    {
        var model = new PendingApprovalNotificationModel(approverName, pendingCount);
        return new NotificationRequest<PendingApprovalNotificationModel>(NotificationType.PendingApprovalNotification, model, attachments);
    }
}