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
/// <typeparam name="T">The type of data to include in the notification</typeparam>
public class NotificationRequest<T> where T : class, ITemplateModel
{
    /// <summary>
    /// Gets the notification type
    /// </summary>
    public NotificationType Type { get; }
    
    /// <summary>
    /// Gets the data to include in the notification
    /// </summary>
    public T Data { get; }
    
    /// <summary>
    /// Gets the attachments to include with the notification
    /// </summary>
    public List<IAttachment> Attachments { get; } = new();
    
    /// <summary>
    /// Creates a new notification request
    /// </summary>
    /// <param name="type">The notification type</param>
    /// <param name="data">The data to include in the notification</param>
    public NotificationRequest(NotificationType type, T data)
    {
        Type = type;
        Data = data ?? throw new ArgumentNullException(nameof(data));
    }
    
    /// <summary>
    /// Adds an attachment to the notification request
    /// </summary>
    /// <param name="attachment">The attachment to add</param>
    /// <returns>The updated notification request</returns>
    public NotificationRequest<T> AddAttachment(IAttachment attachment)
    {
        if (attachment == null)
        {
            throw new ArgumentNullException(nameof(attachment));
        }
        
        Attachments.Add(attachment);
        return this;
    }
}