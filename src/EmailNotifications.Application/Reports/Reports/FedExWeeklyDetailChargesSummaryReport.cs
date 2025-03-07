using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using EmailNotifications.Application.Common.Notifications.Interfaces;
using EmailNotifications.Application.Common.Notifications.Models;
using EmailNotifications.Application.Reports.Interfaces;
using Microsoft.Extensions.Logging;

namespace EmailNotifications.Application.Reports.Reports;

public class FedExWeeklyDetailChargesSummaryReport(
    INotificationService notificationService,
    ILogger<FedExWeeklyDetailChargesSummaryReport> logger)
    : IFedExWeeklyDetailChargesSummaryReport
{
    public async Task<bool> SendAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // Create a simple CSV file (headers only for this example)
            var dateRange = $"{DateTime.Now.AddDays(-7):yyyyMMdd}_to_{DateTime.Now:yyyyMMdd}";
            var fileName = $"FedExWeeklyDetailChargesSummary_{dateRange}.csv";
            var csvBytes = Encoding.UTF8.GetBytes("TrackingNumber,ShipDate,DeliveryDate,ServiceType,Weight,Zone,BusinessUnit,Cost");

            // Create the attachment
            var attachment = new FileAttachment
            {
                FileName = fileName,
                Content = csvBytes,
                ContentType = "text/csv",
                IsInline = false
            };

            // Create the notification request with the attachment in one step
            var request = NotificationTemplates.FedExWeeklyDetailChargesSummary(
                reportTitle: "FedEx Weekly Detail Charges Summary",
                dateRange: $"{DateTime.Now.AddDays(-7):yyyy-MM-dd} to {DateTime.Now:yyyy-MM-dd}",
                totalShipments: 0,
                totalCost: 0m,
                attachments: new List<IAttachment> { attachment }
            );

            // Send the notification
            return await notificationService.SendAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error sending FedEx Weekly Detail Charges Summary report");
            return false;
        }
    }
} 