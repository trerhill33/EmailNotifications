using EmailNotifications.Application.Reports;
using Microsoft.AspNetCore.Mvc;

namespace EmailNotifications.Api.Controllers;

/// <summary>
/// Controller for triggering reports
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IDailyReportService _dailyReportService;
    private readonly IWeeklyReportService _weeklyReportService;
    private readonly ILogger<ReportsController> _logger;

    /// <summary>
    /// Initializes a new instance of the ReportsController
    /// </summary>
    public ReportsController(
        IDailyReportService dailyReportService,
        IWeeklyReportService weeklyReportService,
        ILogger<ReportsController> logger)
    {
        _dailyReportService = dailyReportService ?? throw new ArgumentNullException(nameof(dailyReportService));
        _weeklyReportService = weeklyReportService ?? throw new ArgumentNullException(nameof(weeklyReportService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    /// <summary>
    /// Generates and sends all daily reports
    /// </summary>
    [HttpPost("daily")]
    public async Task<IActionResult> GenerateDailyReports(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Starting daily report generation");
            await _dailyReportService.GenerateAndSendAllReportsAsync(cancellationToken);
            _logger.LogInformation("Daily report generation completed successfully");
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating daily reports");
            return StatusCode(500, "An error occurred while generating daily reports");
        }
    }

    /// <summary>
    /// Generates and sends all weekly reports
    /// </summary>
    [HttpPost("weekly")]
    public async Task<IActionResult> GenerateWeeklyReports(CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Starting weekly report generation");
            await _weeklyReportService.GenerateAndSendAllReportsAsync(cancellationToken);
            _logger.LogInformation("Weekly report generation completed successfully");
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating weekly reports");
            return StatusCode(500, "An error occurred while generating weekly reports");
        }
    }

    /// <summary>
    /// Generates and sends a specific daily report
    /// </summary>
    [HttpPost("daily/{reportType}")]
    public async Task<IActionResult> GenerateSpecificDailyReport(
        [FromRoute] string reportType,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Starting specific daily report generation: {ReportType}", reportType);

            switch (reportType.ToLower())
            {
                case "fedex-remittance-summary":
                    await _dailyReportService.SendFedExRemittanceSummaryAsync(cancellationToken);
                    break;
                case "fedex-remittance-details":
                    await _dailyReportService.SendFedExRemittanceDetailsAsync(cancellationToken);
                    break;
                case "fedex-file-missing":
                    await _dailyReportService.SendFedExFileMissingNotificationAsync(cancellationToken);
                    break;
                case "reassigned-tracking":
                    await _dailyReportService.SendReassignedTrackingNumbersReportAsync(cancellationToken);
                    break;
                case "delayed-invoices":
                    await _dailyReportService.SendDelayedInvoicesReportAsync(cancellationToken);
                    break;
                case "pending-approval":
                    await _dailyReportService.SendPendingApprovalNotificationsAsync(cancellationToken);
                    break;
                default:
                    return BadRequest($"Unknown report type: {reportType}");
            }

            _logger.LogInformation("Specific daily report generation completed successfully: {ReportType}", reportType);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating specific daily report: {ReportType}", reportType);
            return StatusCode(500, $"An error occurred while generating the {reportType} report");
        }
    }

    /// <summary>
    /// Generates and sends a specific weekly report
    /// </summary>
    [HttpPost("weekly/{reportType}")]
    public async Task<IActionResult> GenerateSpecificWeeklyReport(
        [FromRoute] string reportType,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Starting specific weekly report generation: {ReportType}", reportType);

            switch (reportType.ToLower())
            {
                case "fedex-weekly-charges":
                    await _weeklyReportService.SendFedExWeeklyChargesSummaryAsync(cancellationToken);
                    break;
                case "fedex-weekly-detail-charges":
                    await _weeklyReportService.SendFedExWeeklyDetailChargesSummaryAsync(cancellationToken);
                    break;
                case "fedex-file-receipt":
                    await _weeklyReportService.SendFedExFileReceiptNotificationAsync(cancellationToken);
                    break;
                case "tracking-by-business-unit":
                    await _weeklyReportService.SendTrackingNumbersByBusinessUnitReportAsync(cancellationToken);
                    break;
                case "invalid-employee-id":
                    await _weeklyReportService.SendInvalidEmployeeIdReportAsync(cancellationToken);
                    break;
                default:
                    return BadRequest($"Unknown report type: {reportType}");
            }

            _logger.LogInformation("Specific weekly report generation completed successfully: {ReportType}",
                reportType);
            return Ok();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating specific weekly report: {ReportType}", reportType);
            return StatusCode(500, $"An error occurred while generating the {reportType} report");
        }
    }
}