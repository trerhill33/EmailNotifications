namespace EmailNotifications.Application.Reports;

/// <summary>
/// Service for generating and sending weekly reports
/// </summary>
public interface IWeeklyReportService : IReportService
{
    /// <summary>
    /// Sends the FedEx Weekly Charges Summary report
    /// </summary>
    Task SendFedExWeeklyChargesSummaryAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Sends the FedEx Weekly Detail Charges Summary report
    /// </summary>
    Task SendFedExWeeklyDetailChargesSummaryAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Sends a notification that Clayton received and processed a file from FedEx
    /// </summary>
    Task SendFedExFileReceiptNotificationAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Sends a report of tracking numbers by business unit/cost center/location
    /// </summary>
    Task SendTrackingNumbersByBusinessUnitReportAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Sends a report of tracking numbers with invalid employee IDs in the reference notes field
    /// </summary>
    Task SendInvalidEmployeeIdReportAsync(CancellationToken cancellationToken = default);
} 