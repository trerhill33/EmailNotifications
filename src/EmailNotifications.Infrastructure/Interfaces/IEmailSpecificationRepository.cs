using EmailNotifications.Domain.Entities;
using EmailNotifications.Domain.Enums;

namespace EmailNotifications.Infrastructure.Interfaces;

/// <summary>
/// Repository interface for email specifications
/// </summary>
public interface IEmailSpecificationRepository
{
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
    /// Adds a recipient group to an email specification
    /// </summary>
    /// <param name="specificationId">The ID of the email specification</param>
    /// <param name="group">The recipient group to add</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The updated email specification</returns>
    Task<EmailSpecification> AddRecipientGroupAsync(Guid specificationId, EmailRecipientGroup group, CancellationToken cancellationToken = default);

    /// <summary>
    /// Updates a recipient group in an email specification
    /// </summary>
    /// <param name="specificationId">The ID of the email specification</param>
    /// <param name="group">The recipient group to update</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The updated email specification</returns>
    Task<EmailSpecification> UpdateRecipientGroupAsync(Guid specificationId, EmailRecipientGroup group, CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes a recipient group from an email specification
    /// </summary>
    /// <param name="specificationId">The ID of the email specification</param>
    /// <param name="groupId">The ID of the recipient group to delete</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The updated email specification</returns>
    Task<EmailSpecification> DeleteRecipientGroupAsync(Guid specificationId, Guid groupId, CancellationToken cancellationToken = default);
}