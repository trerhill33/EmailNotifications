namespace EmailNotifications.Application.Models;

/// <summary>
/// Represents a file attachment for a notification
/// </summary>
public class FileAttachment : IAttachment
{
    /// <summary>
    /// Gets the name of the file
    /// </summary>
    public string FileName { get; }
    
    /// <summary>
    /// Gets the content of the file
    /// </summary>
    public byte[] Content { get; }
    
    /// <summary>
    /// Gets the content type of the file
    /// </summary>
    public string ContentType { get; }
    
    /// <summary>
    /// Gets whether the attachment should be displayed inline
    /// </summary>
    public bool IsInline { get; }
    
    /// <summary>
    /// Gets the content ID for inline attachments
    /// </summary>
    public string? ContentId { get; }
    
    /// <summary>
    /// Creates a new file attachment
    /// </summary>
    public FileAttachment(string fileName, byte[] content, string contentType, bool isInline = false, string? contentId = null)
    {
        FileName = fileName;
        Content = content;
        ContentType = contentType;
        IsInline = isInline;
        ContentId = contentId;
    }
} 