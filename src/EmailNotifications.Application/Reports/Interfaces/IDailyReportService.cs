namespace EmailNotifications.Application.Reports;

/// <summary>
/// Service for generating and sending daily reports
/// </summary>
public interface IDailyReportService : IReportService
{
    /// <summary>
    /// Sends the FedEx Remittance Summary report
    /// </summary>
    Task SendFedExRemittanceSummaryAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Sends the FedEx Remittance Details report
    /// </summary>
    Task SendFedExRemittanceDetailsAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Sends a notification when a FedEx file is missing
    /// </summary>
    Task SendFedExFileMissingNotificationAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Sends a report of all tracking numbers reassigned to specific organizations
    /// </summary>
    Task SendReassignedTrackingNumbersReportAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Sends a report of invoices that are delayed in processing (≥ 10 days after invoice date)
    /// </summary>
    Task SendDelayedInvoicesReportAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Sends notifications for batches that need approval
    /// </summary>
    Task SendPendingApprovalNotificationsAsync(CancellationToken cancellationToken = default);
} 