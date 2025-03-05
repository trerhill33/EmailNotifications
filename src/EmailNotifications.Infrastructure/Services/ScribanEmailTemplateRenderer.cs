using EmailNotifications.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Scriban;
using Scriban.Runtime;

namespace EmailNotifications.Infrastructure.Services;

/// <summary>
/// Scriban-based implementation of the template renderer
/// </summary>
internal sealed class ScribanEmailTemplateRenderer : ITemplateRenderer
{
    private readonly ILogger<ScribanEmailTemplateRenderer> _logger;
    private readonly string _wrapperTemplate;

    public ScribanEmailTemplateRenderer(
        ILogger<ScribanEmailTemplateRenderer> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _wrapperTemplate = LoadWrapperTemplate();
    }

    /// <summary>
    /// Starts the template rendering process
    /// </summary>
    public async Task<string> StartRenderingAsync<TModel>(string templateContent, TModel model, CancellationToken cancellationToken = default) where TModel : class
    {
        ArgumentNullException.ThrowIfNull(templateContent);
        ArgumentNullException.ThrowIfNull(model);

        _logger.LogDebug("Starting template rendering with model of type {ModelType}", typeof(TModel).Name);
        
        // First render the content template
        var renderedContent = await RenderBodyContentAsync(templateContent, model, cancellationToken);
        
        // Then render the wrapper with the content
        var result = await RenderWithWrapperAsync(renderedContent, cancellationToken);
        
        _logger.LogDebug("Template rendering completed successfully");
        return result;
    }

    /// <summary>
    /// Renders the body content of the template
    /// </summary>
    private async Task<string> RenderBodyContentAsync<TModel>(string templateContent, TModel model, CancellationToken cancellationToken) where TModel : class
    {
        try
        {
            _logger.LogDebug("Parsing content template");
            var contentTemplate = Template.Parse(templateContent);
            
            // Create script object for model binding
            var scriptObject = new ScriptObject();
            
            // Add each property of the model to the script object
            foreach (var property in typeof(TModel).GetProperties())
            {
                var value = property.GetValue(model);
                scriptObject.Add(property.Name, value);
            }
            
            _logger.LogDebug("Rendering content template");
            var renderedContent = await contentTemplate.RenderAsync(scriptObject);
            
            if (string.IsNullOrEmpty(renderedContent))
            {
                _logger.LogWarning("Content template rendered an empty string");
            }
            
            return renderedContent;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rendering content template");
            throw;
        }
    }

    /// <summary>
    /// Renders the content within the wrapper template
    /// </summary>
    private async Task<string> RenderWithWrapperAsync(string bodyContent, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogDebug("Loading wrapper template");
            var wrapperTemplateContent = _wrapperTemplate;
            
            if (string.IsNullOrEmpty(wrapperTemplateContent))
            {
                _logger.LogWarning("No wrapper template found, returning content as-is");
                return bodyContent;
            }
            
            _logger.LogDebug("Parsing wrapper template");
            var wrapperTemplate = Template.Parse(wrapperTemplateContent);
            
            // Create script object for wrapper binding
            var wrapperScriptObject = new ScriptObject();
            wrapperScriptObject.Add("body", bodyContent);
            
            _logger.LogDebug("Rendering with wrapper template");
            var result = await wrapperTemplate.RenderAsync(wrapperScriptObject);
            
            if (string.IsNullOrEmpty(result))
            {
                _logger.LogWarning("Wrapper template rendered an empty string, returning original content");
                return bodyContent;
            }
            
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rendering wrapper template");
            throw;
        }
    }

    private string LoadWrapperTemplate()
    {
        try
        {
            _logger.LogDebug("Loading email wrapper template from embedded resource");

            var assembly = typeof(ScribanEmailTemplateRenderer).Assembly;
            var resourceName = "EmailNotifications.Infrastructure.Templates.email_wrapper_template.html";

            using var stream = assembly.GetManifestResourceStream(resourceName);
            if (stream == null)
            {
                var error = $"Email wrapper template not found in embedded resources. Resource name: {resourceName}";
                _logger.LogError(error);
                throw new FileNotFoundException(error);
            }

            using var reader = new StreamReader(stream);
            var template = reader.ReadToEnd();
            
            _logger.LogInformation("Successfully loaded wrapper template from embedded resource");
            return template;
        }
        catch (Exception ex) when (ex is not FileNotFoundException)
        {
            _logger.LogError(ex, "Error loading wrapper template");
            throw;
        }
    }
}