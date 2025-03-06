using EmailNotifications.Application.Common.Notifications.Interfaces;
using EmailNotifications.Application.Reports.Models;
using EmailNotifications.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace EmailNotifications.Application.Reports.Services;

/// <summary>
/// Implementation of the weekly report service
/// </summary>
public class WeeklyReportService : IWeeklyReportService
{
    private readonly INotificationService _notificationService;
    private readonly ILogger<WeeklyReportService> _logger;

    /// <summary>
    /// Initializes a new instance of the WeeklyReportService class
    /// </summary>
    /// <param name="notificationService">The notification service for sending emails</param>
    /// <param name="logger">The logger instance</param>
    public WeeklyReportService(
        INotificationService notificationService,
        ILogger<WeeklyReportService> logger)
    {
        _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Generates and sends all weekly reports
    /// </summary>
    public async Task GenerateAndSendAllReportsAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting weekly report generation");
        
        try
        {
            await SendFedExWeeklyChargesSummaryAsync(cancellationToken);
            await SendFedExWeeklyDetailChargesSummaryAsync(cancellationToken);
            await SendFedExFileReceiptNotificationAsync(cancellationToken);
            await SendTrackingNumbersByBusinessUnitReportAsync(cancellationToken);
            await SendInvalidEmployeeIdReportAsync(cancellationToken);
            
            _logger.LogInformation("All weekly reports generated and sent successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating weekly reports");
            throw;
        }
    }

    /// <summary>
    /// Sends the FedEx Weekly Charges Summary report
    /// </summary>
    public async Task SendFedExWeeklyChargesSummaryAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Generating FedEx Weekly Charges Summary report");
        
        try
        {
            // Create a simple model for the report
            var model = new FedExWeeklyChargesSummaryModel();
            
            // Send the notification
            await _notificationService.SendNotificationAsync(
                NotificationType.FedExWeeklyChargesSummary,
                model,
                null,
                cancellationToken);
            
            _logger.LogInformation("FedEx Weekly Charges Summary report sent successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending FedEx Weekly Charges Summary report");
            throw;
        }
    }

    /// <summary>
    /// Sends the FedEx Weekly Detail Charges Summary report
    /// </summary>
    public async Task SendFedExWeeklyDetailChargesSummaryAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Generating FedEx Weekly Detail Charges Summary report");
        
        try
        {
            // Create a simple model for the report
            var model = new FedExWeeklyDetailChargesSummaryModel();
            
            // Send the notification
            await _notificationService.SendNotificationAsync(
                NotificationType.FedExWeeklyDetailChargesSummary,
                model,
                null,
                cancellationToken);
            
            _logger.LogInformation("FedEx Weekly Detail Charges Summary report sent successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending FedEx Weekly Detail Charges Summary report");
            throw;
        }
    }

    /// <summary>
    /// Sends a notification that Clayton received and processed a file from FedEx
    /// </summary>
    public async Task SendFedExFileReceiptNotificationAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Generating FedEx File Receipt notification");
        
        try
        {
            // Create a simple model for the notification
            var model = new FedExFileReceiptModel();
            
            // Send the notification
            await _notificationService.SendNotificationAsync(
                NotificationType.FedExFileReceipt,
                model,
                null,
                cancellationToken);
            
            _logger.LogInformation("FedEx File Receipt notification sent successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending FedEx File Receipt notification");
            throw;
        }
    }

    /// <summary>
    /// Sends a report of tracking numbers by business unit/cost center/location
    /// </summary>
    public async Task SendTrackingNumbersByBusinessUnitReportAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Generating Tracking Numbers By Business Unit report");
        
        try
        {
            // Create a simple model for the report
            var model = new TrackingNumbersByBusinessUnitModel();
            
            // Send the notification
            await _notificationService.SendNotificationAsync(
                NotificationType.WeeklyTrackingByBusinessUnit,
                model,
                null,
                cancellationToken);
            
            _logger.LogInformation("Tracking Numbers By Business Unit report sent successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending Tracking Numbers By Business Unit report");
            throw;
        }
    }

    /// <summary>
    /// Sends a report of tracking numbers with invalid employee IDs in the reference notes field
    /// </summary>
    public async Task SendInvalidEmployeeIdReportAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Generating Invalid Employee ID report");
        
        try
        {
            // Create a simple model for the report
            var model = new InvalidEmployeeIdModel();
            
            // Send the notification
            await _notificationService.SendNotificationAsync(
                NotificationType.InvalidEmployeeIdSummary,
                model,
                null,
                cancellationToken);
            
            _logger.LogInformation("Invalid Employee ID report sent successfully");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending Invalid Employee ID report");
            throw;
        }
    }
} 