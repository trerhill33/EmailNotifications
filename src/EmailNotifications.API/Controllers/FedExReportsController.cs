using EmailNotifications.Application.Common.Results;
using EmailNotifications.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace EmailNotifications.Api.Controllers;

[ApiController]
[Route("api/fedex-reports")]
public class FedExReportsController : ControllerBase
{
    private readonly FedExReportService _fedExReportService;
    private readonly ILogger<FedExReportsController> _logger;

    public FedExReportsController(
        FedExReportService fedExReportService,
        ILogger<FedExReportsController> logger)
    {
        _fedExReportService = fedExReportService ?? throw new ArgumentNullException(nameof(fedExReportService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    [HttpPost("weekly-charges-summary")]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SendWeeklyChargesSummary(CancellationToken cancellationToken)
    {
        try
        {
            bool success = await _fedExReportService.SendWeeklyChargesSummaryAsync(cancellationToken);

            if (success)
            {
                return Ok(Result<string>.Success("Weekly FedEx charges summary report sent successfully"));
            }

            return StatusCode(500, Result<string>.Fail("Failed to generate and send weekly FedEx charges summary report"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, Result<string>.Fail($"An error occurred: {ex.Message}"));
        }
    }

    [HttpPost("monthly-delivery-performance")]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Result<string>), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> SendMonthlyDeliveryPerformance(CancellationToken cancellationToken)
    {
        try
        {
            bool success = await _fedExReportService.SendMonthlyDeliveryPerformanceAsync(cancellationToken);

            if (success)
            {
                return Ok(Result<string>.Success("Monthly FedEx delivery performance report sent successfully"));
            }

            return StatusCode(500,
                Result<string>.Fail("Failed to generate and send monthly FedEx delivery performance report"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, Result<string>.Fail($"An error occurred: {ex.Message}"));
        }
    }
} 