namespace EmailNotifications.Application.Reports;

/// <summary>
/// Base interface for all reporting services
/// </summary>
public interface IReportService
{
    /// <summary>
    /// Generates and sends all reports for this service
    /// </summary>
    Task GenerateAndSendAllReportsAsync(CancellationToken cancellationToken = default);
} 