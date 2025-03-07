using EmailNotifications.Application.Common.Notifications.Interfaces;
using EmailNotifications.Application.Common.Notifications.Models;
using EmailNotifications.Application.Reports.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace EmailNotifications.Application.Reports.Reports;

public class FedExFileMissingReport(
    INotificationService notificationService,
    ILogger<FedExFileMissingReport> logger)
    : IFedExFileMissingReport
{
    public async Task<bool> SendAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var request = NotificationTemplates.FedExFileMissing(
                expectedDate: DateTime.Now.ToString("yyyy-MM-dd"),
                fileType: "Daily Charges"
            );
            
            return await notificationService.SendAsync(request, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error sending FedEx File Missing notification");
            return false;
        }
    }
} 