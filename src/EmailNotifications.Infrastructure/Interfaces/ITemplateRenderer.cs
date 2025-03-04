using EmailNotifications.Domain.Enums;

namespace EmailNotifications.Infrastructure.Interfaces;

/// <summary>
/// Interface for rendering email templates
/// </summary>
public interface ITemplateRenderer
{
    /// <summary>
    /// Renders an email template with the specified model
    /// </summary>
    /// <typeparam name="TModel">The type of the model to use for rendering</typeparam>
    /// <param name="notificationType">The notification type that determines which template to use</param>
    /// <param name="model">The model to use for rendering the template</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The rendered template as a string</returns>
    Task<string> RenderAsync<TModel>(NotificationType notificationType, TModel model, CancellationToken cancellationToken = default) where TModel : class;
}