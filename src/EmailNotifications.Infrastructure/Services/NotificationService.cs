using EmailNotifications.Application.Interfaces;
using EmailNotifications.Application.Models;
using EmailNotifications.Domain.Entities;
using EmailNotifications.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Text.Json;

namespace EmailNotifications.Infrastructure.Services;

public class NotificationService(
    IEmailSender emailSender,
    ITemplateRenderer templateRenderer,
    IEmailSpecificationRepository emailSpecificationRepository,
    ILogger<NotificationService> logger)
    : INotificationService
{
    private readonly IEmailSender _emailSender = emailSender ?? throw new ArgumentNullException(nameof(emailSender));
    private readonly ITemplateRenderer _templateRenderer = templateRenderer ?? throw new ArgumentNullException(nameof(templateRenderer));
    private readonly IEmailSpecificationRepository _emailSpecificationRepository = emailSpecificationRepository ?? throw new ArgumentNullException(nameof(emailSpecificationRepository));
    private readonly ILogger<NotificationService> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task<bool> SendAsync<T>(NotificationRequest<T> request, CancellationToken cancellationToken = default) where T : class, ITemplateModel
    {
        ArgumentNullException.ThrowIfNull(request);

        try
        {
            _logger.LogInformation("Processing notification request of type {NotificationType}", request.Type);

            // Get the notification type ID
            var notificationTypeId = (int)request.Type;

            _logger.LogDebug("Rendering template for notification type ID {NotificationTypeId}", notificationTypeId);

            // Render the template with the model - this will get the email spec from the repository
            var htmlBody = await _templateRenderer.RenderAsync(notificationTypeId, request.Data, cancellationToken);

            _logger.LogDebug("Creating email message for notification type ID {NotificationTypeId}", notificationTypeId);

            // Create the email message
            var emailMessage = await CreateEmailMessageAsync(notificationTypeId, htmlBody, null, cancellationToken);

            _logger.LogInformation("Sending email to {Recipients} for notification type {NotificationType}",
                string.Join(", ", emailMessage.To.Select(t => t.Address)),
                request.Type);

            // Send the email with retry logic
            var result = await _emailSender.SendEmailWithRetryAsync(emailMessage, cancellationToken: cancellationToken);

            if (result)
            {
                _logger.LogInformation("Successfully sent notification of type {NotificationType} to {Recipients}",
                    request.Type,
                    string.Join(", ", emailMessage.To.Select(t => t.Address)));
            }
            else
            {
                _logger.LogWarning("Failed to send notification of type {NotificationType} to {Recipients}",
                    request.Type,
                    string.Join(", ", emailMessage.To.Select(t => t.Address)));
            }

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send notification of type {NotificationType}", request.Type);
            return false;
        }
    }

    private async Task<EmailMessage> CreateEmailMessageAsync(int notificationTypeId, string htmlBody, string? textBody, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogDebug("Retrieving email specification for notification type ID {NotificationTypeId}", notificationTypeId);

            // Get the email specification for the notification type
            var emailSpec = await GetEmailSpecificationAsync(notificationTypeId, cancellationToken);

            if (emailSpec == null)
            {
                _logger.LogError("Email specification not found for notification type ID {NotificationTypeId}", notificationTypeId);
                throw new InvalidOperationException($"Email specification not found for notification type ID {notificationTypeId}");
            }

            var from = new MailAddress(emailSpec.FromAddress, emailSpec.FromName);

            MailAddress? replyTo = null;
            if (!string.IsNullOrEmpty(emailSpec.ReplyToAddress))
            {
                replyTo = new MailAddress(emailSpec.ReplyToAddress);
            }

            var to = new List<MailAddress>();
            var cc = new List<MailAddress>();
            var bcc = new List<MailAddress>();

            _logger.LogDebug("Processing recipient groups for email specification {SpecificationName}", emailSpec.Name);

            // Process recipient groups
            foreach (var group in emailSpec.RecipientGroups)
            {
                foreach (var recipient in group.Recipients)
                {
                    var mailAddress = new MailAddress(recipient.EmailAddress, recipient.DisplayName);

                    switch (recipient.Type)
                    {
                        case Domain.Enums.RecipientType.To:
                            to.Add(mailAddress);
                            break;
                        case Domain.Enums.RecipientType.CC:
                            cc.Add(mailAddress);
                            break;
                        case Domain.Enums.RecipientType.BCC:
                            bcc.Add(mailAddress);
                            break;
                        default:
                            var exception = new ArgumentOutOfRangeException
                            {
                                HelpLink = null,
                                HResult = 0,
                                Source = null
                            };
                            throw exception;
                    }
                }
            }

            _logger.LogDebug("Created email message with {ToCount} To recipients, {CcCount} CC recipients, and {BccCount} BCC recipients",
                to.Count, cc.Count, bcc.Count);

            return new EmailMessage
            {
                Subject = emailSpec.Subject,
                HtmlBody = htmlBody,
                TextBody = textBody,
                From = from,
                ReplyTo = replyTo,
                To = to,
                Cc = cc,
                Bcc = bcc,
                Priority = emailSpec.Priority
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating email message for notification type ID {NotificationTypeId}", notificationTypeId);
            throw;
        }
    }

    private async Task<EmailSpecification?> GetEmailSpecificationAsync(int notificationTypeId, CancellationToken cancellationToken)
    {
        var emailSpec = await _emailSpecificationRepository.GetByNotificationTypeIdAsync(notificationTypeId, cancellationToken);

        if (emailSpec == null)
        {
            _logger.LogError("Email specification not found for notification type ID {NotificationTypeId}", notificationTypeId);
            return null;
        }

        if (emailSpec.IsActive) return emailSpec;

        _logger.LogWarning("Email specification for notification type ID {NotificationTypeId} is inactive", notificationTypeId);
        return null;

    }
}