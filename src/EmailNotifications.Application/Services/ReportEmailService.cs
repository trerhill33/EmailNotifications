using EmailNotifications.Application.Common.Reports.Models;
using EmailNotifications.Application.Interfaces;
using EmailNotifications.Application.Models;
using EmailNotifications.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace EmailNotifications.Application.Services;

/// <summary>
/// Service for sending report emails with attachments
/// </summary>
public class ReportEmailService(
    INotificationService notificationService,
    ILogger<ReportEmailService> logger)
{
    /// <summary>
    /// Sends a report as an email attachment
    /// </summary>
    /// <param name="notificationType">The type of notification to send</param>
    /// <param name="reportName">The name of the report</param>
    /// <param name="reportContent">The content of the report file</param>
    /// <param name="contentType">The MIME type of the report file</param>
    /// <param name="fileName">The filename for the attachment</param>
    /// <param name="recordCount">The number of records in the report</param>
    /// <param name="notes">Any additional notes about the report</param>
    /// <param name="cancellationToken">A token to cancel the operation</param>
    /// <returns>True if the email was sent successfully, false otherwise</returns>
    public async Task<bool> Check(
        NotificationType notificationType,
        string reportName,
        byte[] reportContent,
        string contentType,
        string fileName,
        int recordCount,
        string? notes = null,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Preparing to send report email for {ReportName}", reportName);
        
        try
        {
            // Create the template model
            var model = new ReportNotificationModel
            {
                ReportDate = DateTime.UtcNow,
                ReportName = reportName,
                RecordCount = recordCount,
                Notes = notes
            };
            
            // Create the attachment
            var attachment = new FileAttachment(
                fileName,
                reportContent,
                contentType);
            
            // Create the notification request
            var request = new NotificationRequest<ReportNotificationModel>(notificationType, model)
            {
                Attachments = new[] { attachment }
            };
            
            // Send the notification
            var result = await notificationService.SendAsync(request, cancellationToken);
            
            if (result)
            {
                logger.LogInformation("Successfully sent report email for {ReportName}", reportName);
            }
            else
            {
                logger.LogWarning("Failed to send report email for {ReportName}", reportName);
            }
            
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error sending report email for {ReportName}", reportName);
            return false;
        }
    }
    
    /// <summary>
    /// Sends multiple reports as email attachments
    /// </summary>
    /// <param name="notificationType">The type of notification to send</param>
    /// <param name="reportName">The name of the report</param>
    /// <param name="attachments">The collection of attachments to send</param>
    /// <param name="recordCount">The total number of records across all reports</param>
    /// <param name="notes">Any additional notes about the reports</param>
    /// <param name="cancellationToken">A token to cancel the operation</param>
    /// <returns>True if the email was sent successfully, false otherwise</returns>
    public async Task<bool> SendMultipleReportsAsync(
        NotificationType notificationType,
        string reportName,
        IEnumerable<FileAttachment> attachments,
        int recordCount,
        string? notes = null,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Preparing to send multiple report email for {ReportName}", reportName);
        
        try
        {
            // Create the template model
            var model = new ReportNotificationModel
            {
                ReportDate = DateTime.UtcNow,
                ReportName = reportName,
                RecordCount = recordCount,
                Notes = notes
            };
            
            // Create the notification request
            var request = new NotificationRequest<ReportNotificationModel>(notificationType, model)
            {
                Attachments = attachments.ToArray()
            };
            
            // Send the notification
            var result = await notificationService.SendAsync(request, cancellationToken);
            
            if (result)
            {
                logger.LogInformation("Successfully sent multiple report email for {ReportName}", reportName);
            }
            else
            {
                logger.LogWarning("Failed to send multiple report email for {ReportName}", reportName);
            }
            
            return result;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error sending multiple report email for {ReportName}", reportName);
            return false;
        }
    }
} 