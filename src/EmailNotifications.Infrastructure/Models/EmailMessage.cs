using System.Net.Mail;

namespace EmailNotifications.Infrastructure.Models;

/// <summary>
/// Represents an email message to be sent
/// </summary>
public sealed record EmailMessage
{
    /// <summary>
    /// Gets or sets the subject of the email
    /// </summary>
    public required string Subject { get; init; }

    /// <summary>
    /// Gets or sets the HTML body of the email
    /// </summary>
    public required string HtmlBody { get; init; }

    /// <summary>
    /// Gets or sets the plain text body of the email
    /// </summary>
    public string? TextBody { get; init; }

    /// <summary>
    /// Gets or sets the sender's email address
    /// </summary>
    public required MailAddress From { get; init; }

    /// <summary>
    /// Gets or sets the reply-to email address
    /// </summary>
    public MailAddress? ReplyTo { get; init; }

    /// <summary>
    /// Gets or sets the collection of recipients in the To field
    /// </summary>
    public required ICollection<MailAddress> To { get; init; } = new List<MailAddress>();

    /// <summary>
    /// Gets or sets the collection of recipients in the CC field
    /// </summary>
    public ICollection<MailAddress> Cc { get; init; } = new List<MailAddress>();

    /// <summary>
    /// Gets or sets the collection of recipients in the BCC field
    /// </summary>
    public ICollection<MailAddress> Bcc { get; init; } = new List<MailAddress>();

    /// <summary>
    /// Gets or sets the priority of the email (1-5, where 1 is highest)
    /// </summary>
    public int Priority { get; init; } = 3;

    /// <summary>
    /// Gets or sets the collection of attachments for the email
    /// </summary>
    public IReadOnlyCollection<EmailAttachment> Attachments { get; init; } = Array.Empty<EmailAttachment>();
}