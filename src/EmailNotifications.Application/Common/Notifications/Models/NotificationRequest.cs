using EmailNotifications.Domain.Enums;

namespace EmailNotifications.Application.Models;

/// <summary>
/// Base interface for notification requests
/// </summary>
public interface INotificationRequest { }

/// <summary>
/// Represents a request to send a notification
/// </summary>
public sealed record NotificationRequest<T> : INotificationRequest where T : ITemplateModel
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
    public IReadOnlyCollection<IAttachment> Attachments { get; init; } = Array.Empty<IAttachment>();
    
    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationRequest{T}"/> class
    /// </summary>
    /// <param name="type">The notification type</param>
    /// <param name="data">The data model for the template</param>
    public NotificationRequest(NotificationType type, T data)
    {
        Type = type;
        Data = data;
    }
}