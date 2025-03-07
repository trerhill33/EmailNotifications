namespace EmailNotifications.Application.Common.Notifications.Interfaces;

/// <summary>
/// Represents an attachment for a notification
/// </summary>
public interface IAttachment
{
    /// <summary>
    /// Gets the name of the file
    /// </summary>
    string FileName { get; }
    
    /// <summary>
    /// Gets the content of the file as a byte array
    /// </summary>
    byte[] Content { get; }
    
    /// <summary>
    /// Gets the content type of the attachment
    /// </summary>
    string ContentType { get; }
    
    /// <summary>
    /// Gets whether the attachment is inline
    /// </summary>
    bool IsInline { get; }
    
    /// <summary>
    /// Gets the content ID of the attachment (for inline attachments)
    /// </summary>
    string? ContentId { get; }
} 