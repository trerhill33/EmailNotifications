using System.Text;
using EmailNotifications.Application.Common.Notifications.Interfaces;
using EmailNotifications.Application.Common.Notifications.Models;
using EmailNotifications.Application.Reports.Interfaces;
using Microsoft.Extensions.Logging;

namespace EmailNotifications.Application.Reports.Reports;

public class FedExRemittanceDetailsReport(
    INotificationService notificationService,
    ILogger<FedExRemittanceDetailsReport> logger)
    : IFedExRemittanceDetailsReport
{
    public async Task<bool> SendAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // Create a simple CSV file (headers only for this example)
            var fileName = $"FedExRemittanceDetails_{DateTime.Now:yyyyMMdd}.csv";
            var csvBytes = Encoding.UTF8.GetBytes("TrackingNumber,Date,Amount,BusinessUnit,ServiceType,Reference");

            // Create the attachment
            var attachment = new FileAttachment
            {
                FileName = fileName,
                Content = csvBytes,
                ContentType = "text/csv",
                IsInline = false
            };

            // Create the notification request with the attachment in one step
            var request = NotificationTemplates.FedExRemittanceDetails(
                reportTitle: "FedEx Daily Remittance Details",
                dateRange: DateTime.Now.ToString("yyyy-MM-dd"),
                totalRemittance: 0m,
                attachments: new List<IAttachment> { attachment }
            );

            // Send the notification
            return await notificationService.SendAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error sending FedEx Remittance Details report");
            return false;
        }
    }
}