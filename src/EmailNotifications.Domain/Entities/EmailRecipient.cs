using EmailNotifications.Domain.Enums;

namespace EmailNotifications.Domain.Entities;

/// <summary>
/// Represents an email recipient for a specific recipient group
/// </summary>
public sealed class EmailRecipient : AuditableEntity
{
    /// <summary>
    /// The email address of the recipient
    /// </summary>
    public required string EmailAddress { get; set; }

    /// <summary>
    /// The display name of the recipient
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// The type of recipient (To, CC, BCC)
    /// </summary>
    public RecipientType Type { get; set; } = RecipientType.To;

    /// <summary>
    /// The ID of the recipient group this recipient belongs to
    /// </summary>
    public int EmailRecipientGroupId { get; set; }

    /// <summary>
    /// The recipient group this recipient belongs to
    /// </summary>
    public EmailRecipientGroup? EmailRecipientGroup { get; set; }
}