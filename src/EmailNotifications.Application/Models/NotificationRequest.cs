using EmailNotifications.Domain.Enums;

namespace EmailNotifications.Application.Models;

/// <summary>
/// Base interface for notification requests
/// </summary>
public interface INotificationRequest { }

/// <summary>
/// Represents a request to send a notification
/// </summary>
public sealed record NotificationRequest<T>(
    NotificationType Type,
    T Data) : INotificationRequest where T : ITemplateModel;