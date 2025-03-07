using EmailNotifications.Application.Common.Notifications.Interfaces;

namespace EmailNotifications.Application.Common.Notifications.Models;

/// <summary>
/// Implementation of IAttachment for CSV files
/// </summary>
public class CsvAttachment : IAttachment
{
    /// <summary>
    /// Gets the name of the CSV file
    /// </summary>
    public string FileName { get; init; } = string.Empty;
    
    /// <summary>
    /// Gets the content of the CSV file as a byte array
    /// </summary>
    public byte[] Content { get; init; } = Array.Empty<byte>();
    
    /// <summary>
    /// Gets the content type of the attachment (typically "text/csv")
    /// </summary>
    public string ContentType { get; init; } = "text/csv";
    
    /// <summary>
    /// Gets whether the attachment is inline (typically false for CSV files)
    /// </summary>
    public bool IsInline { get; init; } = false;
    
    /// <summary>
    /// Gets the content ID of the attachment (typically null for CSV files)
    /// </summary>
    public string? ContentId { get; init; } = null;
} 