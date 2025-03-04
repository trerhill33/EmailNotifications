using System.Collections.Concurrent;
using System.Security.Cryptography;
using System.Text;
using EmailNotifications.Domain.Enums;
using EmailNotifications.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using Scriban;

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

            var template = _templateCache.GetOrAdd(
                GetTemplateKey(emailSpec.HtmlBody),
                key =>
                {
                    _logger.LogDebug("Parsing and caching template for specification {SpecificationName}", emailSpec.Name);
                    return Template.Parse(emailSpec.HtmlBody);
                });

            if (template.HasErrors)
            {
                var errors = string.Join(", ", template.Messages.Select(m => m.Message));
                _logger.LogError("Template parsing errors: {Errors}", errors);
                throw new InvalidOperationException($"Template parsing errors: {errors}");
            }

            var result = await template.RenderAsync(new { model });
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