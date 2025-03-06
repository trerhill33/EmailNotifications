using EmailNotifications.Application.Common.Notifications.Interfaces;
using EmailNotifications.Domain.Enums;

namespace EmailNotifications.Application.Common.Notifications.Models;

/// <summary>
/// Base model for all report templates
/// </summary>
public sealed record ReportTemplateModel(
    string ReportTitle,
    string DateRange) : ITemplateModel;

/// <summary>
/// Model for FedEx weekly charges summary report
/// </summary>
public sealed record FedExWeeklyChargesSummaryModel(
    string ReportTitle,
    string DateRange,
    int TotalShipments,
    decimal TotalCost) : ITemplateModel;

/// <summary>
/// Model for FedEx weekly detail charges summary report
/// </summary>
public sealed record FedExWeeklyDetailChargesSummaryModel(
    string ReportTitle,
    string DateRange,
    int TotalShipments,
    decimal TotalCost) : ITemplateModel;

/// <summary>
/// Model for FedEx remittance summary report
/// </summary>
public sealed record FedExRemittanceSummaryModel(
    string ReportTitle,
    string DateRange,
    decimal TotalRemittance) : ITemplateModel;

/// <summary>
/// Model for FedEx remittance details report
/// </summary>
public sealed record FedExRemittanceDetailsModel(
    string ReportTitle,
    string DateRange,
    decimal TotalRemittance) : ITemplateModel;

/// <summary>
/// Model for FedEx file receipt notification
/// </summary>
public sealed record FedExFileReceiptModel(
    string FileName,
    string ReceivedDate,
    string ProcessedDate) : ITemplateModel;

/// <summary>
/// Model for FedEx file missing notification
/// </summary>
public sealed record FedExFileMissingModel(
    string ExpectedDate,
    string FileType) : ITemplateModel;

/// <summary>
/// Model for reassigned tracking numbers report
/// </summary>
public sealed record ReassignedTrackingNumbersModel(
    string ReportTitle,
    string ReportDate,
    int TotalReassigned) : ITemplateModel;

/// <summary>
/// Model for delayed invoices report
/// </summary>
public sealed record DelayedInvoicesModel(
    string ReportTitle,
    string ReportDate,
    int TotalDelayed) : ITemplateModel;

/// <summary>
/// Model for pending approval notification
/// </summary>
public sealed record PendingApprovalNotificationModel(
    string ApproverName,
    int PendingCount) : ITemplateModel;

/// <summary>
/// Model for tracking numbers by business unit report
/// </summary>
public sealed record TrackingNumbersByBusinessUnitModel(
    string ReportTitle,
    string DateRange,
    int TotalBusinessUnits) : ITemplateModel;

/// <summary>
/// Model for invalid employee ID summary report
/// </summary>
public sealed record InvalidEmployeeIdModel(
    string ReportTitle,
    string ReportDate,
    int TotalInvalidIds) : ITemplateModel; 