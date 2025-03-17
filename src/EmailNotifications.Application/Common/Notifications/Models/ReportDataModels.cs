using EmailNotifications.Application.Common.Notifications.Interfaces;

namespace EmailNotifications.Application.Common.Notifications.Models;

/// <summary>
/// Model for FedEx weekly charges summary report
/// </summary>
public sealed record WeeklySummary(
    string ReportTitle,
    decimal TotalCost) : ITemplateDataModel;

/// <summary>
/// Model for pending approval notification
/// </summary>
public sealed record PendingApprovalsModel(
    string ApproverName,
    int PendingCount) : ITemplateDataModel;
    