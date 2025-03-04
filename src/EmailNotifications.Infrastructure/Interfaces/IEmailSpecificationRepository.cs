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
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A list of all email specifications</returns>
    Task<IEnumerable<EmailSpecification>> GetAllAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets an email specification by notification type with all related entities
    /// </summary>
    /// <param name="notificationType">The notification type to find the specification for</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The email specification if found, null otherwise</returns>
    Task<EmailSpecification?> GetByNotificationTypeAsync(NotificationType notificationType, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a new email specification
    /// </summary>
    /// <param name="specification">The email specification to add</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The added email specification</returns>
    Task<EmailSpecification> AddAsync(EmailSpecification specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates an existing email specification
    /// </summary>
    /// <param name="specification">The email specification to update</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The updated email specification</returns>
    Task<EmailSpecification> UpdateAsync(EmailSpecification specification, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes an email specification
    /// </summary>
    /// <param name="id">The ID of the email specification to delete</param>
    /// <param name="cancellationToken">The cancellation token</param>
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets all recipients for a specification's email group
    /// </summary>
    /// <param name="specificationId">The ID of the email specification</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>A list of all recipients in the group</returns>
    Task<IEnumerable<EmailRecipient>> GetRecipientsAsync(Guid specificationId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Adds a recipient to a specification's email group
    /// </summary>
    /// <param name="specificationId">The ID of the email specification</param>
    /// <param name="recipient">The recipient to add</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The added recipient</returns>
    Task<EmailRecipient> AddRecipientAsync(Guid specificationId, EmailRecipient recipient, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a recipient in a specification's email group
    /// </summary>
    /// <param name="specificationId">The ID of the email specification</param>
    /// <param name="recipient">The recipient to update</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The updated recipient</returns>
    Task<EmailRecipient> UpdateRecipientAsync(Guid specificationId, EmailRecipient recipient, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a recipient from a specification's email group
    /// </summary>
    /// <param name="specificationId">The ID of the email specification</param>
    /// <param name="email">The email of the recipient to delete</param>
    /// <param name="cancellationToken">The cancellation token</param>
    Task DeleteRecipientAsync(Guid specificationId, string email, CancellationToken cancellationToken = default);
}