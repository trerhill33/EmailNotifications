using EmailNotifications.Application.Common.Notifications.Models;

namespace EmailNotifications.Application.Common.Notifications.Interfaces;

public interface INotificationService
{
    Task<bool> SendAsync<T>(
        NotificationRequest<T> request, 
        CancellationToken cancellationToken = default) 
        where T : class, ITemplateModel;
}