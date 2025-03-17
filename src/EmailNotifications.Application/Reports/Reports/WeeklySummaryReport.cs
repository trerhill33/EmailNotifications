using System.Text;
using EmailNotifications.Application.Common.Notifications.Interfaces;
using EmailNotifications.Application.Common.Notifications.Models;
using Microsoft.Extensions.Logging;

namespace EmailNotifications.Application.Reports.Reports;

public interface IWeeklySummaryReport
{
    Task<bool> GenerateAsync(CancellationToken cancellationToken = default);
}

public class WeeklySummaryReport(
    INotificationService notificationService,
    ILogger<WeeklySummaryReport> logger) : IWeeklySummaryReport
{
    public async Task<bool> GenerateAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // Create a simple CSV file (headers only for this example)
            var fileName = $"WeeklySummary{DateTime.Now:yyyyMMdd}.csv";
            var csvBytes = Encoding.UTF8.GetBytes("reportTitle,totalShipments,totalCost");

            // Create the attachment
            var attachment = new FileAttachment
            {
                FileName = fileName,
                Content = csvBytes,
                ContentType = "text/csv",
                IsInline = false
            };

            // Create the notification request with the attachment in one step
            var request = NotificationFactory.WeeklySummary(
                reportTitle: "New Weekly Summary",
                totalShipments: 3,
                totalCost: 43,
                attachments: new List<IAttachment> { attachment }
            );

            // Send the notification
            return await notificationService.SendAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error sending Pending Approval notifications");
            return false;
        }
    }
} 