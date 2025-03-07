using EmailNotifications.Application.Common.Notifications.Interfaces;

namespace EmailNotifications.Application.Common.Notifications.Models;

/// <summary>
/// Represents a file attachment for a notification
/// </summary>
public class FileAttachment : IAttachment
{
    /// <summary>
    /// Gets the name of the file
    /// </summary>
    public required string FileName  { get; init; }
    
    /// <summary>
    /// Gets the content of the file
    /// </summary>
    public required byte[] Content  { get; init; }
    
    /// <summary>
    /// Gets the content type of the file
    /// </summary>
    public required string ContentType  { get; init; }
    
    /// <summary>
    /// Gets whether the attachment should be displayed inline
    /// </summary>
    public bool IsInline  { get; init; }
    
    /// <summary>
    /// Gets the content ID for inline attachments
    /// </summary>
    public string? ContentId { get; init; }
} 