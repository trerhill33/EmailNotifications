using EmailNotifications.Application.Common.Notifications.Interfaces;
using EmailNotifications.Domain.Enums;

namespace EmailNotifications.Application.Common.Notifications.Models;

/// <summary>
/// Base interface for notification requests
/// </summary>
public interface INotificationRequest { }

/// <summary>
/// Represents a request to send a notification
/// </summary>
public sealed record NotificationRequest<T> : INotificationRequest where T : ITemplateDataModel
{
    /// <summary>
    /// Gets the notification type
    /// </summary>
    public NotificationType Type { get; }

    /// <summary>
    /// Gets the data model for the template
    /// </summary>
    public T Data { get; }

    /// <summary>
    /// Gets the collection of attachments for the notification
    /// </summary>
    public IReadOnlyCollection<IAttachment> Attachments { get; } = Array.Empty<IAttachment>();

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationRequest{T}"/> class without attachments
    /// </summary>
    /// <param name="type">The notification type</param>
    /// <param name="data">The data model for the template</param>
    public NotificationRequest(NotificationType type, T data)
    {
        Type = type;
        Data = data;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationRequest{T}"/> class with attachments
    /// </summary>
    /// <param name="type">The notification type</param>
    /// <param name="data">The data model for the template</param>
    /// <param name="attachments">The collection of attachments to include with the notification</param>
    public NotificationRequest(NotificationType type, T data, IReadOnlyCollection<IAttachment> attachments)
    {
        Type = type;
        Data = data;
        Attachments = attachments;
    }
}