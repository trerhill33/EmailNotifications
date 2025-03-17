using EmailNotifications.Application.Reports.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmailNotifications.Api.Controllers;

/// <summary>
/// Controller for triggering reports
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ReportsController(
    IWeeklyReportService weeklyReportService,
    ILogger<ReportsController> logger)
    : ControllerBase
{
    private readonly IWeeklyReportService _weeklyReportService = weeklyReportService ?? throw new ArgumentNullException(nameof(weeklyReportService));
    private readonly ILogger<ReportsController> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

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
                case "pending-approvals":
                    await _weeklyReportService.GeneratePendingApprovalReport(cancellationToken);
                    break;
                case "weekly-summary":
                    await _weeklyReportService.GenerateWeeklySummaryReport(cancellationToken);
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