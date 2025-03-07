namespace EmailNotifications.Application.Reports;

public interface IReportService
{
    Task<bool> GenerateAndSendAllReportsAsync(CancellationToken cancellationToken = default);
} 