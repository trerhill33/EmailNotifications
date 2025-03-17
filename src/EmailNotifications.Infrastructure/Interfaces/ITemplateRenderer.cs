namespace EmailNotifications.Infrastructure.Interfaces;

/// <summary>
/// Interface for rendering templates
/// </summary>
public interface ITemplateRenderer
{
    /// <summary>
    /// Starts the template rendering process
    /// </summary>
    Task<string> StartRenderingAsync<TModel>(string templateContent, TModel model, CancellationToken cancellationToken = default) where TModel : class;
}