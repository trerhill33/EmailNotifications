using EmailNotifications.Domain.Enums;

namespace EmailNotifications.API.Models;

/// <summary>
/// DTO for reading an email recipient
/// </summary>
public class EmailRecipientResponseDto
{
    /// <summary>
    /// The unique identifier of the email recipient
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The email address of the recipient
    /// </summary>
    public string EmailAddress { get; set; } = string.Empty;

    /// <summary>
    /// The display name of the recipient
    /// </summary>
    public string? DisplayName { get; set; }

    /// <summary>
    /// The type of recipient (To, Cc, Bcc)
    /// </summary>
    public RecipientType Type { get; set; }

    /// <summary>
    /// The ID of the recipient group this recipient belongs to
    /// </summary>
    public Guid EmailRecipientGroupId { get; set; }

    /// <summary>
    /// When the recipient was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Who created the recipient
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// When the recipient was last modified
    /// </summary>
    public DateTime? LastModifiedAt { get; set; }

    /// <summary>
    /// Who last modified the recipient
    /// </summary>
    public string? LastModifiedBy { get; set; }
} 