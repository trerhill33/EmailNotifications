namespace EmailNotifications.Infrastructure.Interfaces;

/// <summary>
/// Interface for rendering templates
/// </summary>
public interface ITemplateRenderer
{
    /// <summary>
    /// Starts the template rendering process
    /// </summary>
    /// <typeparam name="TModel">The type of the model</typeparam>
    /// <param name="templateContent">The template content</param>
    /// <param name="model">The model to bind to the template</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The rendered template</returns>
    Task<string> StartRenderingAsync<TModel>(string templateContent, TModel model, CancellationToken cancellationToken = default) where TModel : class;
}