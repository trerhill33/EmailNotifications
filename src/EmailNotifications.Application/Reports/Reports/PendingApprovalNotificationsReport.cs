using System.Text;
using EmailNotifications.Application.Common.Notifications.Interfaces;
using EmailNotifications.Application.Common.Notifications.Models;
using Microsoft.Extensions.Logging;

namespace EmailNotifications.Application.Reports.Reports;

public interface IPendingApprovalReport
{
    Task<bool> GenerateAsync(CancellationToken cancellationToken = default);
}

public class PendingApprovalReport(
    INotificationService notificationService,
    ILogger<PendingApprovalReport> logger) : IPendingApprovalReport
{
    public async Task<bool> GenerateAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            // Create a simple CSV file (headers only for this example)
            var fileName = $"PendingApprovals_{DateTime.Now:yyyyMMdd}.csv";
            var csvBytes = Encoding.UTF8.GetBytes("RequestId,SubmittedDate,RequestType,RequestorName,Amount");

            // Create the attachment
            var attachment = new FileAttachment
            {
                FileName = fileName,
                Content = csvBytes,
                ContentType = "text/csv",
                IsInline = false
            };

            // Create the notification request with the attachment in one step
            var request = NotificationFactory.PendingApproval(
                approverName: "Jane Approver",
                pendingCount: 3,
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