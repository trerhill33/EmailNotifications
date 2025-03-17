using EmailNotifications.Domain.Entities;
using EmailNotifications.Domain.Enums;

namespace EmailNotifications.Infrastructure.Interfaces;

/// <summary>
/// Repository interface for email specifications
/// </summary>
public interface IEmailSpecificationRepository
{
    /// <summary>
    /// Gets all email specifications with their related entities
    /// </summary>
    Task<IEnumerable<EmailSpecification>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an email specification by ID with all related entities
    /// </summary>
    Task<EmailSpecification?> GetByIdAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an email specification by notification type with all related entities
    /// </summary>
    Task<EmailSpecification?> GetByNotificationTypeAsync(NotificationType notificationType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new email specification
    /// </summary>
    Task<EmailSpecification> AddAsync(EmailSpecification specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing email specification
    /// </summary>
    Task<EmailSpecification> UpdateAsync(EmailSpecification specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an email specification
    /// </summary>
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all recipients for a specification's email group
    /// </summary>
    Task<IEnumerable<EmailRecipient>> GetRecipientsAsync(int specificationId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a recipient to a specification's email group
    /// </summary>
    Task<EmailRecipient> AddRecipientAsync(int specificationId, EmailRecipient recipient, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a recipient in a specification's email group
    /// </summary>
    Task<EmailRecipient> UpdateRecipientAsync(int specificationId, EmailRecipient recipient, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a recipient from a specification's email group
    /// </summary>
    Task DeleteRecipientAsync(int specificationId, string email, CancellationToken cancellationToken = default);
}