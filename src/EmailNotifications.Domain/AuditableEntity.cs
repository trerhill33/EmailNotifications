namespace EmailNotifications.Domain.Common;

/// <summary>
/// Base class for all auditable entities in the system
/// </summary>
public abstract class AuditableEntity
{
    /// <summary>
    /// Unique identifier for the entity
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Date and time when the entity was created
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// User who created the entity
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// Date and time when the entity was last modified
    /// </summary>
    public DateTime? LastModifiedAt { get; set; }

    /// <summary>
    /// User who last modified the entity
    /// </summary>
    public string? LastModifiedBy { get; set; }
}