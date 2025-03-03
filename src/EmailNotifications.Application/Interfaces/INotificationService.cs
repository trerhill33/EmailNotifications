using EmailNotifications.Application.Models;

namespace EmailNotifications.Application.Interfaces;

public interface INotificationService
{
    Task<bool> SendAsync<T>(NotificationRequest<T> request, CancellationToken cancellationToken = default) where T : class, ITemplateModel;
}