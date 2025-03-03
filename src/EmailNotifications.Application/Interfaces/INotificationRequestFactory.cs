using EmailNotifications.Application.Enums;
using EmailNotifications.Application.Models;

namespace EmailNotifications.Application.Interfaces;

/// <summary>
/// Factory for creating notification requests
/// </summary>
public interface INotificationRequestFactory
{
    NotificationRequest<T> Create<T>(NotificationType type, T data) where T : class, ITemplateModel;
}