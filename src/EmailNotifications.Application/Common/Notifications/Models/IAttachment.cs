namespace EmailNotifications.Application.Models;

/// <summary>
/// Represents an attachment that can be included with a notification
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
    /// Gets the content type (MIME type) of the file
    /// </summary>
    string ContentType { get; }
    
    /// <summary>
    /// Gets whether the attachment should be displayed inline
    /// </summary>
    bool IsInline { get; }
    
    /// <summary>
    /// Gets the optional content ID for inline attachments
    /// </summary>
    string? ContentId { get; }
} 