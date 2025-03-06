using EmailNotifications.Application.Common.Notifications.Interfaces;
using EmailNotifications.Application.Common.Notifications.Models;
using Microsoft.Extensions.Logging;

namespace EmailNotifications.Application.Reports.Services;

/// <summary>
/// Implementation of the weekly report service
/// </summary>
public sealed class WeeklyReportService(
    INotificationService notificationService,
    ILogger<WeeklyReportService> logger) : IWeeklyReportService
{
    private readonly INotificationService _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
    private readonly ILogger<WeeklyReportService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// Generates and sends all weekly reports
    /// </summary>
    public async Task<bool> GenerateAndSendAllReportsAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting weekly report generation");
        
        try
        {
            var results = new[]
            {
                await SendFedExWeeklyChargesSummaryAsync(cancellationToken),
                await SendFedExWeeklyDetailChargesSummaryAsync(cancellationToken),
                await SendFedExFileReceiptNotificationAsync(cancellationToken),
                await SendTrackingNumbersByBusinessUnitReportAsync(cancellationToken),
                await SendInvalidEmployeeIdSummaryAsync(cancellationToken)
            };
            
            var allSuccessful = results.All(r => r);
            if (allSuccessful)
            {
                _logger.LogInformation("All weekly reports generated and sent successfully");
            }
            else
            {
                _logger.LogWarning("Some weekly reports failed to send");
            }
            
            return allSuccessful;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating weekly reports");
            return false;
        }
    }

    /// <summary>
    /// Sends the FedEx Weekly Charges Summary report
    /// </summary>
    public async Task<bool> SendFedExWeeklyChargesSummaryAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Generating FedEx Weekly Charges Summary report");
            
            var result = await _notificationService.SendAsync(NotificationTemplates.FedExWeeklyChargesSummary(
                reportTitle: "FedEx Weekly Charges Summary",
                dateRange: $"{DateTime.Now.AddDays(-7):yyyy-MM-dd} to {DateTime.Now:yyyy-MM-dd}",
                totalShipments: 0, // TODO: Replace with actual value
                totalCost: 0.00m // TODO: Replace with actual value
            ), cancellationToken);

            if (result)
            {
                _logger.LogInformation("FedEx Weekly Charges Summary report sent successfully");
            }
            else
            {
                _logger.LogWarning("Failed to send FedEx Weekly Charges Summary report");
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending FedEx Weekly Charges Summary report");
            return false;
        }
    }

    /// <summary>
    /// Sends the FedEx Weekly Detail Charges Summary report
    /// </summary>
    public async Task<bool> SendFedExWeeklyDetailChargesSummaryAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Generating FedEx Weekly Detail Charges Summary report");
            
            var result = await _notificationService.SendAsync(NotificationTemplates.FedExWeeklyDetailChargesSummary(
                reportTitle: "FedEx Weekly Detail Charges Summary",
                dateRange: $"{DateTime.Now.AddDays(-7):yyyy-MM-dd} to {DateTime.Now:yyyy-MM-dd}",
                totalShipments: 0, // TODO: Replace with actual value
                totalCost: 0.00m // TODO: Replace with actual value
            ), cancellationToken);

            if (result)
            {
                _logger.LogInformation("FedEx Weekly Detail Charges Summary report sent successfully");
            }
            else
            {
                _logger.LogWarning("Failed to send FedEx Weekly Detail Charges Summary report");
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending FedEx Weekly Detail Charges Summary report");
            return false;
        }
    }

    /// <summary>
    /// Sends a notification when a FedEx file is received
    /// </summary>
    public async Task<bool> SendFedExFileReceiptNotificationAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Generating FedEx File Receipt notification");
            
            var result = await _notificationService.SendAsync(NotificationTemplates.FedExFileReceipt(
                fileName: "WeeklyCharges.txt",
                receivedDate: DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                processedDate: DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
            ), cancellationToken);

            if (result)
            {
                _logger.LogInformation("FedEx File Receipt notification sent successfully");
            }
            else
            {
                _logger.LogWarning("Failed to send FedEx File Receipt notification");
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending FedEx File Receipt notification");
            return false;
        }
    }

    /// <summary>
    /// Sends a report of tracking numbers by business unit
    /// </summary>
    public async Task<bool> SendTrackingNumbersByBusinessUnitReportAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Generating Tracking Numbers by Business Unit report");
            
            var result = await _notificationService.SendAsync(NotificationTemplates.TrackingNumbersByBusinessUnit(
                reportTitle: "Weekly Tracking Numbers by Business Unit",
                dateRange: $"{DateTime.Now.AddDays(-7):yyyy-MM-dd} to {DateTime.Now:yyyy-MM-dd}",
                totalBusinessUnits: 0 // TODO: Replace with actual value
            ), cancellationToken);

            if (result)
            {
                _logger.LogInformation("Tracking Numbers by Business Unit report sent successfully");
            }
            else
            {
                _logger.LogWarning("Failed to send Tracking Numbers by Business Unit report");
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending Tracking Numbers by Business Unit report");
            return false;
        }
    }
    
    /// <summary>
    /// Sends a summary of invalid employee IDs
    /// </summary>
    public async Task<bool> SendInvalidEmployeeIdSummaryAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Generating Invalid Employee ID Summary report");
            
            var result = await _notificationService.SendAsync(NotificationTemplates.InvalidEmployeeId(
                reportTitle: "Weekly Invalid Employee ID Summary",
                reportDate: DateTime.Now.ToString("yyyy-MM-dd"),
                totalInvalidIds: 0 // TODO: Replace with actual value
            ), cancellationToken);

            if (result)
            {
                _logger.LogInformation("Invalid Employee ID Summary report sent successfully");
            }
            else
            {
                _logger.LogWarning("Failed to send Invalid Employee ID Summary report");
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending Invalid Employee ID Summary report");
            return false;
        }
    }
} 