using EmailNotifications.Application.Interfaces;
using EmailNotifications.Application.Models;
using EmailNotifications.Domain.Entities;
using EmailNotifications.Domain.Enums;
using EmailNotifications.Infrastructure.Interfaces;
using Microsoft.Extensions.Logging;
using System.Net.Mail;
using System.Text.Json;
using EmailNotifications.Infrastructure.Models;

namespace EmailNotifications.Infrastructure.Services;

/// <summary>
/// Service for handling email notifications
/// </summary>
public class NotificationService(
    IEmailSender emailSender,
    ITemplateRenderer templateRenderer,
    IEmailSpecificationRepository emailSpecificationRepository,
    ILogger<NotificationService> logger)
    : INotificationService
{
    /// <summary>
    /// Sends an email notification
    /// </summary>
    /// <typeparam name="T">The type of the template model</typeparam>
    /// <param name="request">The notification request</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>True if the notification was sent successfully, false otherwise</returns>
    public async Task<bool> SendAsync<T>(NotificationRequest<T> request, CancellationToken cancellationToken = default) where T : class, ITemplateModel
    {
        try
        {
            logger.LogDebug("Rendering template for notification type {NotificationType}", request.Type);

            // Convert Application layer NotificationType to Domain layer NotificationType
            var domainNotificationType = (Domain.Enums.NotificationType)request.Type;

            var htmlBody = await templateRenderer.RenderAsync(domainNotificationType, request.Data, cancellationToken);

            logger.LogDebug("Creating email message for notification type {NotificationType}", request.Type);

            var emailMessage = await CreateEmailMessageAsync(domainNotificationType, htmlBody, null, cancellationToken);

            await emailSender.SendEmailWithRetryAsync(
                emailMessage,
                maxRetryAttempts: 3,
                retryDelayMilliseconds: 1000,
                cancellationToken);

            logger.LogInformation("Successfully sent notification for type {NotificationType}", request.Type);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error sending notification for type {NotificationType}", request.Type);
            return false;
        }
    }

    private async Task<EmailMessage> CreateEmailMessageAsync(NotificationType notificationType, string htmlBody, string? textBody, CancellationToken cancellationToken)
    {
        try
        {
            logger.LogDebug("Retrieving email specification for notification type {NotificationType}", notificationType);

            var emailSpec = await GetEmailSpecificationAsync(notificationType, cancellationToken);
            if (emailSpec == null)
            {
                logger.LogError("Email specification not found for notification type {NotificationType}", notificationType);
                throw new InvalidOperationException($"Email specification not found for notification type {notificationType}");
            }

            return new EmailMessage
            {
                From = new MailAddress(emailSpec.FromAddress, emailSpec.FromName),
                ReplyTo = emailSpec.ReplyToAddress != null ? new MailAddress(emailSpec.ReplyToAddress) : null,
                Subject = emailSpec.Subject,
                HtmlBody = htmlBody,
                TextBody = textBody ?? emailSpec.TextBody,
                Priority = emailSpec.Priority,
                To = new List<MailAddress>() // Required by EmailMessage
            };
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating email message for notification type {NotificationType}", notificationType);
            throw;
        }
    }

    private async Task<EmailSpecification?> GetEmailSpecificationAsync(NotificationType notificationType, CancellationToken cancellationToken)
    {
        try
        {
            var emailSpec = await emailSpecificationRepository.GetByNotificationTypeAsync(notificationType, cancellationToken);
            if (emailSpec == null)
            {
                logger.LogError("Email specification not found for notification type {NotificationType}", notificationType);
                throw new InvalidOperationException($"Email specification not found for notification type {notificationType}");
            }

            if (!emailSpec.IsActive)
            {
                logger.LogWarning("Email specification for notification type {NotificationType} is inactive", notificationType);
            }

            return emailSpec;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving email specification for notification type {NotificationType}", notificationType);
            throw;
        }
    }
}