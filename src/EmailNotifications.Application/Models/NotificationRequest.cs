using EmailNotifications.Application.Enums;

namespace EmailNotifications.Application.Models;

/// <summary>
/// Base interface for notification requests
/// </summary>
public interface INotificationRequest { }

/// <summary>
/// Represents a request to send a notification
/// </summary>
/// <typeparam name="T">The type of the template model</typeparam>
public sealed record NotificationRequest<T>(
    NotificationType Type,
    T Data) : INotificationRequest where T : ITemplateModel;