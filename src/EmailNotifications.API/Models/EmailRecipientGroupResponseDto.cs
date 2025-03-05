namespace EmailNotifications.API.Models;

/// <summary>
/// DTO for reading an email recipient group
/// </summary>
public class EmailRecipientGroupResponseDto
{
    /// <summary>
    /// The unique identifier of the recipient group
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// The name of the recipient group
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// The description of the recipient group
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The ID of the email specification this group belongs to
    /// </summary>
    public Guid EmailSpecificationId { get; set; }

    /// <summary>
    /// When the group was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// Who created the group
    /// </summary>
    public string CreatedBy { get; set; } = string.Empty;

    /// <summary>
    /// When the group was last modified
    /// </summary>
    public DateTime? LastModifiedAt { get; set; }

    /// <summary>
    /// Who last modified the group
    /// </summary>
    public string? LastModifiedBy { get; set; }
} 