using EmailNotifications.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Scriban;
using Scriban.Runtime;
using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using EmailNotifications.Application.Enums;

namespace EmailNotifications.Infrastructure.Services;

/// <summary>
/// Scriban-based implementation of the template renderer
/// </summary>
internal sealed class ScribanEmailTemplateRenderer : ITemplateRenderer
{
    private readonly ILogger<ScribanEmailTemplateRenderer> _logger;
    private readonly IEmailSpecificationRepository _emailSpecificationRepository;
    private readonly string _wrapperTemplate;
    private readonly ConcurrentDictionary<string, Template> _templateCache = new();

    public ScribanEmailTemplateRenderer(
        ILogger<ScribanEmailTemplateRenderer> logger,
        IEmailSpecificationRepository emailSpecificationRepository)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _emailSpecificationRepository = emailSpecificationRepository ?? throw new ArgumentNullException(nameof(emailSpecificationRepository));
        _wrapperTemplate = LoadWrapperTemplate();
    }

    public async Task<string> RenderAsync<TModel>(int notificationTypeId, TModel model, CancellationToken cancellationToken = default)
        where TModel : class
    {
        ArgumentNullException.ThrowIfNull(model);

        try
        {
            _logger.LogDebug("Starting template rendering for notification type ID: {NotificationTypeId}", notificationTypeId);

            // Step 1: Get the email specification from the repository
            var emailSpec = await _emailSpecificationRepository.GetByNotificationTypeIdAsync(notificationTypeId, cancellationToken);
            if (emailSpec == null)
            {
                _logger.LogError("Email specification not found for notification type ID: {NotificationTypeId}", notificationTypeId);
                throw new InvalidOperationException($"Email specification not found for notification type ID: {notificationTypeId}");
            }

            _logger.LogDebug("Retrieved email specification {SpecificationName} for notification type ID: {NotificationTypeId}",
                emailSpec.Name, notificationTypeId);

            // Step 2: Parse and cache the template
            var template = _templateCache.GetOrAdd(
                GetTemplateKey(emailSpec.HtmlBody),
                key =>
                {
                    _logger.LogDebug("Parsing and caching template for specification {SpecificationName}", emailSpec.Name);
                    return Template.Parse(emailSpec.HtmlBody);
                });

            if (template.HasErrors)
            {
                _logger.LogError("Template parsing errors for specification {SpecificationName}: {Errors}",
                    emailSpec.Name, string.Join("; ", template.Messages));
                throw new InvalidOperationException($"Template parsing failed: {string.Join("; ", template.Messages)}");
            }

            // Step 3: Create template context and render
            var context = new TemplateContext();
            var scriptObject = new ScriptObject();
            scriptObject.Import(model);
            context.PushGlobal(scriptObject);

            _logger.LogDebug("Rendering content for specification {SpecificationName}", emailSpec.Name);
            var notificationHtml = await template.RenderAsync(context);

            // Step 4: Apply wrapper template
            var wrapperTemplate = Template.Parse(_wrapperTemplate);
            var wrapperContext = new TemplateContext();
            var wrapperScriptObject = new ScriptObject();
            wrapperScriptObject.Add("Content", notificationHtml);
            wrapperContext.PushGlobal(wrapperScriptObject);

            _logger.LogDebug("Applying wrapper template to rendered content");
            var finalHtml = await wrapperTemplate.RenderAsync(wrapperContext);

            _logger.LogInformation("Successfully rendered template for notification type ID: {NotificationTypeId}", notificationTypeId);
            return finalHtml;
        }
        catch (Exception ex) when (ex is not InvalidOperationException)
        {
            _logger.LogError(ex, "Error rendering template for notification type ID: {NotificationTypeId}", notificationTypeId);
            throw;
        }
    }

    private static string GetTemplateKey(string template)
    {
        using var sha256 = SHA256.Create();
        var hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(template));
        return Convert.ToBase64String(hash);
    }

    private string LoadWrapperTemplate()
    {
        try
        {
            _logger.LogDebug("Loading email wrapper template");

            // Load from the Infrastructure/Templates directory
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Templates", "email_wrapper_template.html");

            // If not found in the bin directory, try the source directory
            if (!File.Exists(path))
            {
                _logger.LogDebug("Wrapper template not found in bin directory, searching in project structure");
                // Try to find it in the project directory structure
                var projectDir = Path.GetDirectoryName(typeof(ScribanEmailTemplateRenderer).Assembly.Location) ?? "";
                while (!string.IsNullOrEmpty(projectDir) && !Directory.Exists(Path.Combine(projectDir, "Templates")))
                {
                    projectDir = Path.GetDirectoryName(projectDir) ?? "";
                }

                if (!string.IsNullOrEmpty(projectDir))
                {
                    path = Path.Combine(projectDir, "Templates", "email_wrapper_template.html");
                }
            }

            if (!File.Exists(path))
            {
                var error = $"Email wrapper template not found. Searched in: {path}";
                _logger.LogError(error);
                throw new FileNotFoundException(error);
            }

            _logger.LogInformation("Successfully loaded wrapper template from {Path}", path);
            return File.ReadAllText(path);
        }
        catch (Exception ex) when (ex is not FileNotFoundException)
        {
            _logger.LogError(ex, "Error loading wrapper template");
            throw;
        }
    }
}