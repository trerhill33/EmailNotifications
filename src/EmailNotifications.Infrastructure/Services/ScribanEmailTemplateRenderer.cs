using EmailNotifications.Domain.Enums;
using EmailNotifications.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Scriban;
using System.Text.Json;
using Scriban.Runtime;

namespace EmailNotifications.Infrastructure.Services;

/// <summary>
/// Scriban-based implementation of the template renderer
/// </summary>
internal sealed class ScribanEmailTemplateRenderer : ITemplateRenderer
{
    private readonly ILogger<ScribanEmailTemplateRenderer> _logger;
    private readonly IEmailSpecificationRepository _emailSpecificationRepository;
    private readonly string _wrapperTemplate;

    public ScribanEmailTemplateRenderer(
        ILogger<ScribanEmailTemplateRenderer> logger,
        IEmailSpecificationRepository emailSpecificationRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _emailSpecificationRepository = emailSpecificationRepository ?? throw new ArgumentNullException(nameof(emailSpecificationRepository));
        _wrapperTemplate = LoadWrapperTemplate();
    }

    public async Task<string> RenderAsync<TModel>(NotificationType notificationType, TModel model, CancellationToken cancellationToken = default) where TModel : class
    {
        try
        {
            _logger.LogDebug("Starting template rendering for notification type: {NotificationType}", notificationType);

            var emailSpec = await _emailSpecificationRepository.GetByNotificationTypeAsync(notificationType, cancellationToken);
            if (emailSpec == null)
            {
                _logger.LogError("Email specification not found for notification type: {NotificationType}", notificationType);
                throw new InvalidOperationException($"Email specification not found for notification type: {notificationType}");
            }

            _logger.LogDebug("Retrieved email specification {SpecificationName} for notification type: {NotificationType}",
                emailSpec.Name, notificationType);

            // Log the model data
            var modelJson = JsonSerializer.Serialize(model);
            _logger.LogDebug("Model data: {ModelData}", modelJson);

            // First render the content template
            var contentTemplate = Template.Parse(emailSpec.HtmlBody);
            if (contentTemplate.HasErrors)
            {
                var errors = string.Join(", ", contentTemplate.Messages.Select(m => m.Message));
                _logger.LogError("Content template parsing errors: {Errors}", errors);
                throw new InvalidOperationException($"Content template parsing errors: {errors}");
            }

            // Create a script object and import the model
            var scriptObject = new ScriptObject();
            scriptObject.Import(model);

            var renderedContent = await contentTemplate.RenderAsync(scriptObject);
            _logger.LogDebug("Rendered content: {Content}", renderedContent);

            // Now render the wrapper template with the content
            var wrapperTemplate = Template.Parse(_wrapperTemplate);
            if (wrapperTemplate.HasErrors)
            {
                var errors = string.Join(", ", wrapperTemplate.Messages.Select(m => m.Message));
                _logger.LogError("Wrapper template parsing errors: {Errors}", errors);
                throw new InvalidOperationException($"Wrapper template parsing errors: {errors}");
            }

            // Create a new script object for the wrapper with the rendered content
            var wrapperScriptObject = new ScriptObject();
            wrapperScriptObject.Import(model);
            wrapperScriptObject["Content"] = renderedContent;

            var result = await wrapperTemplate.RenderAsync(wrapperScriptObject);
            _logger.LogDebug("Final rendered result: {Result}", result);

            if (string.IsNullOrWhiteSpace(result))
            {
                _logger.LogWarning("Template rendered to empty string for notification type: {NotificationType}", notificationType);
            }

            _logger.LogInformation("Successfully rendered template for notification type: {NotificationType}", notificationType);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error rendering template for notification type: {NotificationType}", notificationType);
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