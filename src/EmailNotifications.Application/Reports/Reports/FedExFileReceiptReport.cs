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

public class FedExFileReceiptReport(
    INotificationService notificationService,
    ILogger<FedExFileReceiptReport> logger)
    : IFedExFileReceiptReport
{
    public async Task<bool> SendAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // Create a simple CSV file (headers only for this example)
            var fileName = $"FedExFileReceipt_{DateTime.Now:yyyyMMdd}.csv";
            var csvBytes = Encoding.UTF8.GetBytes("FileName,FileType,ReceivedDate,ProcessedDate,Status");

            // Create the attachment
            var attachment = new FileAttachment
            {
                FileName = fileName,
                Content = csvBytes,
                ContentType = "text/csv",
                IsInline = false
            };

            // Create the notification request with the attachment in one step
            var request = NotificationTemplates.FedExFileReceipt(
                fileName: "FedEx_Charges_20230101.csv",
                receivedDate: DateTime.Now.AddHours(-1).ToString("yyyy-MM-dd HH:mm"),
                processedDate: DateTime.Now.ToString("yyyy-MM-dd HH:mm"),
                attachments: new List<IAttachment> { attachment }
            );

            // Send the notification
            return await notificationService.SendAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error sending FedEx File Receipt notification");
            return false;
        }
    }
} 