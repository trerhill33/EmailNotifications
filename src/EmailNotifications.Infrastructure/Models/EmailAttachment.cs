using System;

namespace EmailNotifications.Infrastructure.Models;

/// <summary>
/// Represents an email attachment with its content and metadata
/// </summary>
public sealed record EmailAttachment
{
    /// <summary>
    /// Gets the name of the file to be displayed to the recipient
    /// </summary>
    public required string FileName { get; init; }

    /// <summary>
    /// Gets the content of the attachment as a read-only memory buffer
    /// </summary>
    public required ReadOnlyMemory<byte> Content { get; init; }

    /// <summary>
    /// Gets the MIME type of the attachment (e.g., "application/pdf", "image/jpeg")
    /// </summary>
    public required string ContentType { get; init; }

    /// <summary>
    /// Gets the optional Content-ID for inline attachments
    /// </summary>
    public string? ContentId { get; init; }

    /// <summary>
    /// Gets whether the attachment should be displayed inline in the email body
    /// </summary>
    public bool IsInline { get; init; }

    /// <summary>
    /// Gets the size of the attachment in bytes
    /// </summary>
    public long Size => Content.Length;
} 