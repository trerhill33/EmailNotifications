using EmailNotifications.Domain.Entities;
using EmailNotifications.Domain.Enums;

namespace EmailNotifications.Infrastructure.Interfaces;

/// <summary>
/// Repository interface for email specifications
/// </summary>
public interface IEmailSpecificationRepository
{
    /// <summary>
    /// Gets an email specification by notification type
    /// </summary>
    /// <param name="notificationType">The notification type to find the specification for</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The email specification if found, null otherwise</returns>
    Task<EmailSpecification?> GetByNotificationTypeAsync(NotificationType notificationType, CancellationToken cancellationToken = default);
}