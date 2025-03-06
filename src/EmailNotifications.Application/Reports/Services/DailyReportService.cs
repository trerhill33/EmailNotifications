using EmailNotifications.Application.Common.Notifications.Interfaces;
using EmailNotifications.Application.Common.Notifications.Models;
using Microsoft.Extensions.Logging;

namespace EmailNotifications.Application.Reports.Services;

/// <summary>
/// Implementation of the daily report service
/// </summary>
public sealed class DailyReportService(
    INotificationService notificationService,
    ILogger<DailyReportService> logger) : IDailyReportService
{
    private readonly INotificationService _notificationService = notificationService ?? throw new ArgumentNullException(nameof(notificationService));
    private readonly ILogger<DailyReportService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    /// <summary>
    /// Generates and sends all daily reports
    /// </summary>
    public async Task<bool> GenerateAndSendAllReportsAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting daily report generation");
        
        try
        {
            var results = new[]
            {
                await SendFedExRemittanceSummaryAsync(cancellationToken),
                await SendFedExRemittanceDetailsAsync(cancellationToken),
                await SendFedExFileMissingNotificationAsync(cancellationToken),
                await SendReassignedTrackingNumbersReportAsync(cancellationToken),
                await SendDelayedInvoicesReportAsync(cancellationToken),
                await SendPendingApprovalNotificationsAsync(cancellationToken)
            };
            
            var allSuccessful = results.All(r => r);
            if (allSuccessful)
            {
                _logger.LogInformation("All daily reports generated and sent successfully");
            }
            else
            {
                _logger.LogWarning("Some daily reports failed to send");
            }
            
            return allSuccessful;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating daily reports");
            return false;
        }
    }

    /// <summary>
    /// Sends the FedEx Remittance Summary report
    /// </summary>
    public async Task<bool> SendFedExRemittanceSummaryAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Generating FedEx Remittance Summary report");
            
            var result = await _notificationService.SendAsync(NotificationTemplates.FedExRemittanceSummary(
                reportTitle: "FedEx Daily Remittance Summary",
                dateRange: DateTime.Now.ToString("yyyy-MM-dd"),
                totalRemittance: 0.00m // TODO: Replace with actual value
            ), cancellationToken);

            if (result)
            {
                _logger.LogInformation("FedEx Remittance Summary report sent successfully");
            }
            else
            {
                _logger.LogWarning("Failed to send FedEx Remittance Summary report");
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending FedEx Remittance Summary report");
            return false;
        }
    }

    /// <summary>
    /// Sends the FedEx Remittance Details report
    /// </summary>
    public async Task<bool> SendFedExRemittanceDetailsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Generating FedEx Remittance Details report");
            
            var result = await _notificationService.SendAsync(NotificationTemplates.FedExRemittanceDetails(
                reportTitle: "FedEx Daily Remittance Details",
                dateRange: DateTime.Now.ToString("yyyy-MM-dd"),
                totalRemittance: 0.00m // TODO: Replace with actual value
            ), cancellationToken);

            if (result)
            {
                _logger.LogInformation("FedEx Remittance Details report sent successfully");
            }
            else
            {
                _logger.LogWarning("Failed to send FedEx Remittance Details report");
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending FedEx Remittance Details report");
            return false;
        }
    }

    /// <summary>
    /// Sends a notification when a FedEx file is missing
    /// </summary>
    public async Task<bool> SendFedExFileMissingNotificationAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Generating FedEx File Missing notification");
            
            var result = await _notificationService.SendAsync(NotificationTemplates.FedExFileMissing(
                expectedDate: DateTime.Now.ToString("yyyy-MM-dd"),
                fileType: "Daily Charges"
            ), cancellationToken);

            if (result)
            {
                _logger.LogInformation("FedEx File Missing notification sent successfully");
            }
            else
            {
                _logger.LogWarning("Failed to send FedEx File Missing notification");
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending FedEx File Missing notification");
            return false;
        }
    }

    /// <summary>
    /// Sends a report of all tracking numbers reassigned to specific organizations
    /// </summary>
    public async Task<bool> SendReassignedTrackingNumbersReportAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Generating Reassigned Tracking Numbers report");
            
            var result = await _notificationService.SendAsync(NotificationTemplates.ReassignedTrackingNumbers(
                reportTitle: "Daily Reassigned Tracking Numbers",
                reportDate: DateTime.Now.ToString("yyyy-MM-dd"),
                totalReassigned: 0 // TODO: Replace with actual value
            ), cancellationToken);

            if (result)
            {
                _logger.LogInformation("Reassigned Tracking Numbers report sent successfully");
            }
            else
            {
                _logger.LogWarning("Failed to send Reassigned Tracking Numbers report");
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending Reassigned Tracking Numbers report");
            return false;
        }
    }

    /// <summary>
    /// Sends a report of invoices that are delayed in processing (≥ 10 days after invoice date)
    /// </summary>
    public async Task<bool> SendDelayedInvoicesReportAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Generating Delayed Invoices report");
            
            var result = await _notificationService.SendAsync(NotificationTemplates.DelayedInvoices(
                reportTitle: "Daily Delayed Invoices Report",
                reportDate: DateTime.Now.ToString("yyyy-MM-dd"),
                totalDelayed: 0 // TODO: Replace with actual value
            ), cancellationToken);

            if (result)
            {
                _logger.LogInformation("Delayed Invoices report sent successfully");
            }
            else
            {
                _logger.LogWarning("Failed to send Delayed Invoices report");
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending Delayed Invoices report");
            return false;
        }
    }

    /// <summary>
    /// Sends notifications for batches that need approval
    /// </summary>
    public async Task<bool> SendPendingApprovalNotificationsAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogInformation("Generating Pending Approval notifications");
            
            var result = await _notificationService.SendAsync(NotificationTemplates.PendingApproval(
                approverName: "System User", // TODO: Replace with actual approver name
                pendingCount: 0 // TODO: Replace with actual value
            ), cancellationToken);

            if (result)
            {
                _logger.LogInformation("Pending Approval notifications sent successfully");
            }
            else
            {
                _logger.LogWarning("Failed to send Pending Approval notifications");
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending Pending Approval notifications");
            return false;
        }
    }
} 