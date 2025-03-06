namespace EmailNotifications.Application.Reports;

/// <summary>
/// Service for generating and sending daily reports
/// </summary>
public interface IDailyReportService : IReportService
{
    /// <summary>
    /// Sends the FedEx Remittance Summary report
    /// </summary>
    Task<bool> SendFedExRemittanceSummaryAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Sends the FedEx Remittance Details report
    /// </summary>
    Task<bool> SendFedExRemittanceDetailsAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Sends a notification when a FedEx file is missing
    /// </summary>
    Task<bool> SendFedExFileMissingNotificationAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Sends a report of all tracking numbers reassigned to specific organizations
    /// </summary>
    Task<bool> SendReassignedTrackingNumbersReportAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Sends a report of invoices that are delayed in processing (≥ 10 days after invoice date)
    /// </summary>
    Task<bool> SendDelayedInvoicesReportAsync(CancellationToken cancellationToken = default);
    
    /// <summary>
    /// Sends notifications for batches that need approval
    /// </summary>
    Task<bool> SendPendingApprovalNotificationsAsync(CancellationToken cancellationToken = default);
} 