namespace EmailNotifications.API.Models;

/// <summary>
/// DTO for creating or updating an email recipient group
/// </summary>
public class EmailRecipientGroupRequestDto
{
    /// <summary>
    /// The name of the recipient group
    /// </summary>
    public required string Name { get; set; }

    /// <summary>
    /// The description of the recipient group
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// The ID of the email specification this group belongs to
    /// </summary>
    public Guid EmailSpecificationId { get; set; }
} 