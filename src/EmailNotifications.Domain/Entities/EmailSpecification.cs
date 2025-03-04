using EmailNotifications.Domain.Common;
using EmailNotifications.Domain.Enums;

namespace EmailNotifications.Domain.Entities;

/// <summary>
/// Represents an email specification (template) that defines the structure and content of an email
/// </summary>
public sealed class EmailSpecification : AuditableEntity
{
    /// <summary>
    /// The name of the email specification
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// The notification type associated with this specification
    /// </summary>
    public NotificationType NotificationType { get; set; }

    /// <summary>
    /// The subject line of the email
    /// </summary>
    public required string Subject { get; set; }

    /// <summary>
    /// The HTML body content of the email
    /// </summary>
    public required string HtmlBody { get; set; }

    /// <summary>
    /// The plain text body content of the email
    /// </summary>
    public string? TextBody { get; set; }

    /// <summary>
    /// The sender's email address
    /// </summary>
    public required string FromAddress { get; set; }

    /// <summary>
    /// The sender's display name
    /// </summary>
    public string? FromName { get; set; }

    /// <summary>
    /// The reply-to email address
    /// </summary>
    public string? ReplyToAddress { get; set; }

    /// <summary>
    /// The priority of the email (1-5, where 1 is highest)
    /// </summary>
    public int Priority { get; set; } = 3;

    /// <summary>
    /// Whether this email specification is active and can be used
    /// </summary>
    public bool IsActive { get; set; } = true;

    /// <summary>
    /// Collection of recipient groups associated with this email specification
    /// </summary>
    public ICollection<EmailRecipientGroup> RecipientGroups { get; set; } = new List<EmailRecipientGroup>();
}