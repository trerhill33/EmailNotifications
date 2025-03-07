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

public class FedExWeeklyChargesSummaryReport(
    INotificationService notificationService,
    ILogger<FedExWeeklyChargesSummaryReport> logger)
    : IFedExWeeklyChargesSummaryReport
{
    public async Task<bool> SendAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // Create a simple CSV file (headers only for this example)
            var dateRange = $"{DateTime.Now.AddDays(-7):yyyyMMdd}_to_{DateTime.Now:yyyyMMdd}";
            var fileName = $"FedExWeeklyChargesSummary_{dateRange}.csv";
            var csvBytes = Encoding.UTF8.GetBytes("Date,ServiceType,BusinessUnit,TrackingNumber,Cost");

            // Create the attachment
            var attachment = new FileAttachment
            {
                FileName = fileName,
                Content = csvBytes,
                ContentType = "text/csv",
                IsInline = false
            };

            // Create the notification request with the attachment in one step
            var request = NotificationTemplates.FedExWeeklyChargesSummary(
                reportTitle: "FedEx Weekly Charges Summary",
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
            logger.LogError(ex, "Error sending FedEx Weekly Charges Summary report");
            return false;
        }
    }
} 