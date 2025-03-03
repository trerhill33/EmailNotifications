namespace EmailNotifications.Infrastructure.Interfaces;

/// <summary>
/// Service for rendering templates
/// </summary>
public interface ITemplateRenderer
{
    /// <summary>
    /// Renders a template for the specified notification type with the provided model
    /// </summary>
    /// <typeparam name="TModel">The type of the model</typeparam>
    /// <param name="notificationTypeId">The notification type ID that determines which template to use</param>
    /// <param name="model">The model to use for rendering</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The rendered template as HTML</returns>
    Task<string> RenderAsync<TModel>(int notificationTypeId, TModel model, CancellationToken cancellationToken = default) where TModel : class;
}