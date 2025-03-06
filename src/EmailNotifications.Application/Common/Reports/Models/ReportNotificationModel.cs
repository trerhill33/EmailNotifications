using EmailNotifications.Application.Models;

namespace EmailNotifications.Application.Common.Reports.Models;

/// <summary>
/// Example model for a report notification
/// </summary>
public class ReportNotificationModel : ITemplateModel
{
    /// <summary>
    /// Gets or sets the report date
    /// </summary>
    public DateTime ReportDate { get; set; }
    
    /// <summary>
    /// Gets or sets the report name
    /// </summary>
    public string ReportName { get; set; } = string.Empty;
    
    /// <summary>
    /// Gets or sets the number of records in the report
    /// </summary>
    public int RecordCount { get; set; }
    
    /// <summary>
    /// Gets or sets any additional notes about the report
    /// </summary>
    public string? Notes { get; set; }
}