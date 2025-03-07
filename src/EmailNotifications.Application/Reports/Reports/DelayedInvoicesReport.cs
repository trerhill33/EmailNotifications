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

public class DelayedInvoicesReport(
    INotificationService notificationService,
    ILogger<DelayedInvoicesReport> logger)
    : IDelayedInvoicesReport
{
    public async Task<bool> SendAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // Create a simple CSV file (headers only for this example)
            var fileName = $"DelayedInvoices_{DateTime.Now:yyyyMMdd}.csv";
            var csvBytes = Encoding.UTF8.GetBytes("InvoiceNumber,InvoiceDate,ExpectedProcessingDate,DaysDelayed,InvoiceAmount,BusinessUnit");

            // Create the attachment
            var attachment = new FileAttachment
            {
                FileName = fileName,
                Content = csvBytes,
                ContentType = "text/csv",
                IsInline = false
            };

            // Create the notification request with the attachment in one step
            var request = NotificationTemplates.DelayedInvoices(
                reportTitle: "Daily Delayed Invoices Report",
                reportDate: DateTime.Now.ToString("yyyy-MM-dd"),
                totalDelayed: 3,
                attachments: new List<IAttachment> { attachment }
            );

            // Send the notification
            return await notificationService.SendAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error sending Delayed Invoices report");
            return false;
        }
    }
} 