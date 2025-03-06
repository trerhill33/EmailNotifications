using EmailNotifications.Application.Reports.Models;
using EmailNotifications.Domain.Enums;
using EmailNotifications.Application.Interfaces;
using Microsoft.Extensions.Logging;

namespace EmailNotifications.Application.Reports.Services;

/// <summary>
/// Implementation of the daily report service
/// </summary>
public class DailyReportService : IDailyReportService
{
    private readonly INotificationService _notificationService;
    private readonly ILogger<DailyReportService> _logger;

    /// <summary>
    /// Initializes a new instance of the DailyReportService class
    /// </summary>
    /// <param name="notificationService">The notification service for sending emails</param>
    /// <param name="logger">The logger instance</param>
    public DailyReportService(
        INotificationService notificationService,
        ILogger<DailyReportService> logger)
    {
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Generates and sends all daily reports
    /// </summary>
    public async Task GenerateAndSendAllReportsAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting daily report generation");
        
        try
        {
            await SendFedExRemittanceSummaryAsync(cancellationToken);
            await SendFedExRemittanceDetailsAsync(cancellationToken);
            await SendFedExFileMissingNotificationAsync(cancellationToken);
            await SendReassignedTrackingNumbersReportAsync(cancellationToken);
            await SendDelayedInvoicesReportAsync(cancellationToken);
            await SendPendingApprovalNotificationsAsync(cancellationToken);
            
            _logger.LogInformation("All daily reports generated and sent successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating daily reports");
            throw;
        }
    }

    /// <summary>
    /// Sends the FedEx Remittance Summary report
    /// </summary>
    public async Task SendFedExRemittanceSummaryAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Generating FedEx Remittance Summary report");
        
        try
        {
            // Create a simple model for the report
            var model = new FedExRemittanceSummaryModel();
            
            // Send the notification
            await _notificationService.SendNotificationAsync(
                NotificationType.FedExWeeklyChargesSummary,
                model,
                null,
                cancellationToken);
            
            _logger.LogInformation("FedEx Remittance Summary report sent successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending FedEx Remittance Summary report");
            throw;
        }
    }

    /// <summary>
    /// Sends the FedEx Remittance Details report
    /// </summary>
    public async Task SendFedExRemittanceDetailsAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Generating FedEx Remittance Details report");
        
        try
        {
            // Create a simple model for the report
            var model = new FedExRemittanceDetailsModel();
            
            // Send the notification
            await _notificationService.SendAsync(
                NotificationType.FedExRemittanceDetails,
                model,
                cancellationToken);
            
            _logger.LogInformation("FedEx Remittance Details report sent successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending FedEx Remittance Details report");
            throw;
        }
    }

    /// <summary>
    /// Sends a notification when a FedEx file is missing
    /// </summary>
    public async Task SendFedExFileMissingNotificationAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Generating FedEx File Missing notification");
        
        try
        {
            // Create a simple model for the notification
            var model = new FedExFileMissingModel();
            
            // Send the notification
            await _notificationService.SendNotificationAsync(
                NotificationType.FedExFileMissing,
                model,
                null,
                cancellationToken);
            
            _logger.LogInformation("FedEx File Missing notification sent successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending FedEx File Missing notification");
            throw;
        }
    }

    /// <summary>
    /// Sends a report of all tracking numbers reassigned to specific organizations
    /// </summary>
    public async Task SendReassignedTrackingNumbersReportAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Generating Reassigned Tracking Numbers report");
        
        try
        {
            // Create a simple model for the report
            var model = new ReassignedTrackingNumbersModel();
            
            // Send the notification
            await _notificationService.SendNotificationAsync(
                NotificationType.DailyReassignedTrackingNumbers,
                model,
                null,
                cancellationToken);
            
            _logger.LogInformation("Reassigned Tracking Numbers report sent successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending Reassigned Tracking Numbers report");
            throw;
        }
    }

    /// <summary>
    /// Sends a report of invoices that are delayed in processing (≥ 10 days after invoice date)
    /// </summary>
    public async Task SendDelayedInvoicesReportAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Generating Delayed Invoices report");
        
        try
        {
            // Create a simple model for the report
            var model = new DelayedInvoicesModel();
            
            // Send the notification
            await _notificationService.SendNotificationAsync(
                NotificationType.DelayedInvoicesReport,
                model,
                null,
                cancellationToken);
            
            _logger.LogInformation("Delayed Invoices report sent successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending Delayed Invoices report");
            throw;
        }
    }

    /// <summary>
    /// Sends notifications for batches that need approval
    /// </summary>
    public async Task SendPendingApprovalNotificationsAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Generating Pending Approval notifications");
        
        try
        {
            // Create a simple model for the notification
            var model = new PendingApprovalNotificationModel();
            
            // Send the notification
            await _notificationService.SendNotificationAsync(
                NotificationType.PendingApprovalNotification,
                model,
                null,
                cancellationToken);
            
            _logger.LogInformation("Pending Approval notifications sent successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending Pending Approval notifications");
            throw;
        }
    }
} 