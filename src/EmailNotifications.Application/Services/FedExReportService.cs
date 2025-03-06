using System.Text;
using EmailNotifications.Application.Interfaces;
using EmailNotifications.Application.Models;
using EmailNotifications.Domain.Enums;
using Microsoft.Extensions.Logging;

namespace EmailNotifications.Application.Services;

/// <summary>
/// Model for FedEx report notification emails
/// </summary>
public class FedExReportModel : ITemplateModel
{
    /// <summary>
    /// Gets or sets the report date
    /// </summary>
    public DateTime ReportDate { get; set; }
    
    /// <summary>
    /// Gets or sets the report title
    /// </summary>
    public string ReportTitle { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the total number of shipments
    /// </summary>
    public int TotalShipments { get; set; }
    
    /// <summary>
    /// Gets or sets the total cost
    /// </summary>
    public decimal TotalCost { get; set; }
    
    /// <summary>
    /// Gets or sets the date range for the report
    /// </summary>
    public string DateRange { get; set; } = string.Empty;
}

/// <summary>
/// Service for generating and sending FedEx reports
/// </summary>
public class FedExReportService
{
    private readonly INotificationService _notificationService;
    private readonly ILogger<FedExReportService> _logger;
    
    /// <summary>
    /// Initializes a new instance of the <see cref="FedExReportService"/> class
    /// </summary>
    public FedExReportService(
        INotificationService notificationService,
        ILogger<FedExReportService> logger)
    {
        _notificationService = notificationService;
        _logger = logger;
    }
    
    /// <summary>
    /// Generates and sends a weekly FedEx charges summary report
    /// </summary>
    public async Task<bool> SendWeeklyChargesSummaryAsync(CancellationToken cancellationToken = default)
    {
        var endDate = DateTime.UtcNow.Date;
        var startDate = endDate.AddDays(-7);
        var dateRange = $"{startDate:MM/dd/yyyy} - {endDate:MM/dd/yyyy}";
        var filename = $"FedEx_Weekly_Charges_Summary_{startDate:yyyyMMdd}_{endDate:yyyyMMdd}.csv";
        return await SendReportAsync("Weekly", NotificationType.FedExWeeklyChargesSummary, startDate, endDate, dateRange, "Weekly FedEx Charges Summary", filename, cancellationToken);
    }
    
    /// <summary>
    /// Generates and sends a monthly FedEx delivery performance report
    /// </summary>
    public async Task<bool> SendMonthlyDeliveryPerformanceAsync(CancellationToken cancellationToken = default)
    {
        var today = DateTime.UtcNow.Date;
        var firstOfCurrentMonth = new DateTime(today.Year, today.Month, 1);
        var startDate = firstOfCurrentMonth.AddMonths(-1);
        var endDate = firstOfCurrentMonth.AddDays(-1);
        var dateRange = $"{startDate:MM/dd/yyyy} - {endDate:MM/dd/yyyy}";
        var filename = $"FedEx_Monthly_Delivery_Performance_{startDate:yyyyMM}.csv";
        return await SendReportAsync("Monthly", NotificationType.FedExMonthlyDeliveryPerformance, startDate, endDate, dateRange, "Monthly FedEx Delivery Performance", filename, cancellationToken);
    }
    
    /// <summary>
    /// Generates and sends a FedEx report
    /// </summary>
    private async Task<bool> SendReportAsync(
        string reportType, 
        NotificationType notificationType, 
        DateTime startDate, 
        DateTime endDate, 
        string dateRange, 
        string reportTitle, 
        string filename, 
        CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Generating {reportType} report");
        
        try
        {
            var (csvData, totalShipments, totalCost) = GenerateSimpleReport(reportType, startDate, endDate);
            var reportBytes = Encoding.UTF8.GetBytes(csvData);
            var model = new FedExReportModel
            {
                ReportDate = DateTime.UtcNow,
                ReportTitle = reportTitle,
                TotalShipments = totalShipments,
                TotalCost = totalCost,
                DateRange = dateRange
            };
            var attachment = new FileAttachment(filename, reportBytes, "text/csv");
            var request = new NotificationRequest<FedExReportModel>(notificationType, model)
            {
                Attachments = new[] { attachment }
            };
            var result = await _notificationService.SendAsync(request, cancellationToken);
            if (result)
            {
                _logger.LogInformation($"Successfully sent {reportType} report");
            }
            else
            {
                _logger.LogWarning($"Failed to send {reportType} report");
            }
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error generating and sending {reportType} report");
            return false;
        }
    }
    
    /// <summary>
    /// Generates a simple report as a CSV string for testing purposes
    /// </summary>
    /// <returns>A tuple containing the CSV data, total shipments, and total cost</returns>
    private (string CsvData, int TotalShipments, decimal TotalCost) GenerateSimpleReport(
        string reportType, 
        DateTime startDate, 
        DateTime endDate)
    {
        string csvData = $@"Report Type: {reportType}
Date Range: {startDate:yyyy-MM-dd} to {endDate:yyyy-MM-dd}
Date,City,Shipments,Cost
{startDate:yyyy-MM-dd},Test City,1,10.00
Total Shipments: 1
Total Cost: $10.00
";
        int totalShipments = 1;
        decimal totalCost = 10.00m;
        return (csvData, totalShipments, totalCost);
    }
}