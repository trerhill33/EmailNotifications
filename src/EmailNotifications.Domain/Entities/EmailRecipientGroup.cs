using EmailNotifications.Domain.Common;

namespace EmailNotifications.Domain.Entities;

/// <summary>
/// Represents a group of email recipients for a specific email specification
/// </summary>
public sealed class EmailRecipientGroup : AuditableEntity
{
    /// <summary>
    /// The name of the recipient group
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// Description of the recipient group
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The ID of the email specification this group is associated with
    /// </summary>
    public Guid EmailSpecificationId { get; set; }

    /// <summary>
    /// The email specification this group is associated with
    /// </summary>
    public EmailSpecification? EmailSpecification { get; set; }

    /// <summary>
    /// Collection of recipients in this group
    /// </summary>
    public ICollection<EmailRecipient> Recipients { get; set; } = new List<EmailRecipient>();
}