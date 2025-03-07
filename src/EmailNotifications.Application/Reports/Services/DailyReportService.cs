using EmailNotifications.Application.Reports.Interfaces;
using Microsoft.Extensions.Logging;

namespace EmailNotifications.Application.Reports.Services;

/// <summary>
/// Implementation of the daily report service
/// </summary>
public sealed class DailyReportService(
    IFedExRemittanceSummaryReport fedExRemittanceSummaryReport,
    IFedExRemittanceDetailsReport fedExRemittanceDetailsReport,
    IFedExFileMissingReport fedExFileMissingReport,
    IReassignedTrackingNumbersReport reassignedTrackingNumbersReport,
    IDelayedInvoicesReport delayedInvoicesReport,
    IPendingApprovalNotificationsReport pendingApprovalNotificationsReport,
    ILogger<DailyReportService> logger) : IDailyReportService
{
    /// <summary>
    /// Generates and sends all daily reports
    /// </summary>
    public async Task<bool> GenerateAndSendAllReportsAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Starting daily report generation");
        
        try
        {
            // 1. Call each individual report method
            var results = new[]
            {
                await SendFedExRemittanceSummaryAsync(cancellationToken),
                await SendFedExRemittanceDetailsAsync(cancellationToken),
                await SendFedExFileMissingNotificationAsync(cancellationToken),
                await SendReassignedTrackingNumbersReportAsync(cancellationToken),
                await SendDelayedInvoicesReportAsync(cancellationToken),
                await SendPendingApprovalNotificationsAsync(cancellationToken)
            };
            
            // 2. Check if all reports were sent successfully
            var allSuccessful = results.All(r => r);
            if (allSuccessful)
            {
                logger.LogInformation("All daily reports generated and sent successfully");
            }
            else
            {
                logger.LogWarning("Some daily reports failed to send");
            }
            
            // 3. Return overall success status
            return allSuccessful;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error generating daily reports");
            return false;
        }
    }

    /// <summary>
    /// Sends the FedEx Remittance Summary report
    /// </summary>
    public async Task<bool> SendFedExRemittanceSummaryAsync(CancellationToken cancellationToken = default)
    {
        return await fedExRemittanceSummaryReport.SendAsync(cancellationToken);
    }

    /// <summary>
    /// Sends the FedEx Remittance Details report
    /// </summary>
    public async Task<bool> SendFedExRemittanceDetailsAsync(CancellationToken cancellationToken = default)
    {
        return await fedExRemittanceDetailsReport.SendAsync(cancellationToken);
    }

    /// <summary>
    /// Sends a notification when a FedEx file is missing
    /// </summary>
    public async Task<bool> SendFedExFileMissingNotificationAsync(CancellationToken cancellationToken = default)
    {
        return await fedExFileMissingReport.SendAsync(cancellationToken);
    }

    /// <summary>
    /// Sends a report of reassigned tracking numbers
    /// </summary>
    public async Task<bool> SendReassignedTrackingNumbersReportAsync(CancellationToken cancellationToken = default)
    {
        return await reassignedTrackingNumbersReport.SendAsync(cancellationToken);
    }

    /// <summary>
    /// Sends a report of delayed invoices
    /// </summary>
    public async Task<bool> SendDelayedInvoicesReportAsync(CancellationToken cancellationToken = default)
    {
        return await delayedInvoicesReport.SendAsync(cancellationToken);
    }

    /// <summary>
    /// Sends notifications for pending approvals
    /// </summary>
    public async Task<bool> SendPendingApprovalNotificationsAsync(CancellationToken cancellationToken = default)
    {
        return await pendingApprovalNotificationsReport.SendAsync(cancellationToken);
    }
} 