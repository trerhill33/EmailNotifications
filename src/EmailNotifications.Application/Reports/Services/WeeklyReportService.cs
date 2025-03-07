using EmailNotifications.Application.Reports.Interfaces;
using Microsoft.Extensions.Logging;

namespace EmailNotifications.Application.Reports.Services;

/// <summary>
/// Implementation of the weekly report service
/// </summary>
public sealed class WeeklyReportService(
    IFedExWeeklyChargesSummaryReport fedExWeeklyChargesSummaryReport,
    IFedExWeeklyDetailChargesSummaryReport fedExWeeklyDetailChargesSummaryReport,
    IFedExFileReceiptReport fedExFileReceiptReport,
    ITrackingNumbersByBusinessUnitReport trackingNumbersByBusinessUnitReport,
    IInvalidEmployeeIdReport invalidEmployeeIdReport,
    ILogger<WeeklyReportService> logger) : IWeeklyReportService
{
    /// <summary>
    /// Generates and sends all weekly reports
    /// </summary>
    public async Task<bool> GenerateAndSendAllReportsAsync(CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Starting weekly report generation");
        
        try
        {
            // 1. Call each individual report method
            var results = new[]
            {
                await SendFedExWeeklyChargesSummaryAsync(cancellationToken),
                await SendFedExWeeklyDetailChargesSummaryAsync(cancellationToken),
                await SendFedExFileReceiptNotificationAsync(cancellationToken),
                await SendTrackingNumbersByBusinessUnitReportAsync(cancellationToken),
                await SendInvalidEmployeeIdSummaryAsync(cancellationToken)
            };
            
            // 2. Check if all reports were sent successfully
            var allSuccessful = results.All(r => r);
            if (allSuccessful)
            {
                logger.LogInformation("All weekly reports generated and sent successfully");
            }
            else
            {
                logger.LogWarning("Some weekly reports failed to send");
            }
            
            // 3. Return overall success status
            return allSuccessful;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error generating weekly reports");
            return false;
        }
    }

    /// <summary>
    /// Sends the FedEx weekly charges summary report
    /// </summary>
    public async Task<bool> SendFedExWeeklyChargesSummaryAsync(CancellationToken cancellationToken = default)
    {
        return await fedExWeeklyChargesSummaryReport.SendAsync(cancellationToken);
    }

    /// <summary>
    /// Sends the FedEx weekly detail charges summary report
    /// </summary>
    public async Task<bool> SendFedExWeeklyDetailChargesSummaryAsync(CancellationToken cancellationToken = default)
    {
        return await fedExWeeklyDetailChargesSummaryReport.SendAsync(cancellationToken);
    }

    /// <summary>
    /// Sends a notification when a FedEx file is received
    /// </summary>
    public async Task<bool> SendFedExFileReceiptNotificationAsync(CancellationToken cancellationToken = default)
    {
        return await fedExFileReceiptReport.SendAsync(cancellationToken);
    }

    /// <summary>
    /// Sends a report of tracking numbers by business unit
    /// </summary>
    public async Task<bool> SendTrackingNumbersByBusinessUnitReportAsync(CancellationToken cancellationToken = default)
    {
        return await trackingNumbersByBusinessUnitReport.SendAsync(cancellationToken);
    }

    /// <summary>
    /// Sends a report of invalid employee IDs
    /// </summary>
    public async Task<bool> SendInvalidEmployeeIdSummaryAsync(CancellationToken cancellationToken = default)
    {
        return await invalidEmployeeIdReport.SendAsync(cancellationToken);
    }
} 