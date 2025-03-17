using EmailNotifications.Application.Common.Notifications.Models;

namespace EmailNotifications.Application.Common.Notifications.Interfaces;

public interface INotificationService
{
    Task<bool> SendAsync<T>(
        SendNotificationRequest<T> request, 
        CancellationToken cancellationToken = default) 
        where T : class, ITemplateDataModel;
}