using EmailNotifications.Domain.Entities;

namespace EmailNotifications.Infrastructure.Interfaces;

public interface IEmailSpecificationRepository
{
    Task<EmailSpecification?> GetByNotificationTypeIdAsync(int notificationTypeId, CancellationToken cancellationToken = default);
}