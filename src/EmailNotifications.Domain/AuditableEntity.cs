using System;

namespace EmailNotifications.Domain;

/// <summary>
/// Base class for all auditable entities in the system
/// </summary>
public abstract class AuditableEntity
{
    /// <summary>
    /// The unique identifier for the entity
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The date and time when the entity was created
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// User who created the entity
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// The date and time when the entity was last modified
    /// </summary>
    public DateTime LastModifiedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// User who last modified the entity
    /// </summary>
    public string? LastModifiedBy { get; set; }
}