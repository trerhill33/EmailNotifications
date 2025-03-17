using EmailNotifications.Application.Reports.Reports;
using Microsoft.Extensions.Logging;

namespace EmailNotifications.Application.Reports.Services;

public interface IWeeklyReportService
{
    Task<bool> GenerateAndSendAllReportsAsync(CancellationToken cancellationToken = default);
    Task<bool> GeneratePendingApprovalReport(CancellationToken cancellationToken = default);
    Task<bool> GenerateWeeklySummaryReport(CancellationToken cancellationToken = default);
}

/// <summary>
/// Implementation of the weekly report service
/// </summary>
public sealed class WeeklyReportService(
    IPendingApprovalReport pendingApprovalNotificationsReport,
    IWeeklySummaryReport weeklySummaryReport,
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
                await GeneratePendingApprovalReport(cancellationToken),
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
    
    public async Task<bool> GeneratePendingApprovalReport(CancellationToken cancellationToken = default)
        => await pendingApprovalNotificationsReport.GenerateAsync(cancellationToken);   
    public async Task<bool> GenerateWeeklySummaryReport(CancellationToken cancellationToken = default)
        => await weeklySummaryReport.GenerateAsync(cancellationToken);
} 