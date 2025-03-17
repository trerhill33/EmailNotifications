using System.Net.Mail;
using EmailNotifications.Application.Common.Notifications.Interfaces;
using EmailNotifications.Application.Common.Notifications.Models;
using EmailNotifications.Domain.Entities;
using EmailNotifications.Domain.Enums;
using EmailNotifications.Infrastructure.Interfaces;
using EmailNotifications.Infrastructure.Models;
using Microsoft.Extensions.Logging;

namespace EmailNotifications.Infrastructure.Services;

/// <summary>
/// Service that handles sending email notifications
/// </summary>
public class NotificationService(
    IEmailSender emailSender,
    ITemplateRenderer templateRenderer,
    IEmailSpecificationRepository emailSpecificationRepository,
    ILogger<NotificationService> logger)
    : INotificationService
{
    /// <summary>
    /// Processes and sends a notification request
    /// </summary>
    public async Task<bool> SendAsync<T>(SendNotificationRequest<T> request, CancellationToken cancellationToken = default)
        where T : class, ITemplateDataModel
    {
        ArgumentNullException.ThrowIfNull(request);
        ArgumentNullException.ThrowIfNull(request.Data);
        
        try
        {
            logger.LogInformation("Starting notification process for type {NotificationType}", request.Type);
            
            // Step 1: Retrieve email specification
            var emailSpec = await GetEmailSpecificationAsync((int)request.Type, cancellationToken);
            if (emailSpec == null)
            {
                logger.LogWarning("No email specification found for notification type {NotificationType}", request.Type);
                return false;
            }
            
            // Step 2: Render the template with provided data
            logger.LogDebug("Rendering template for email specification {Id}", emailSpec.Id);
            var htmlBody = await templateRenderer.StartRenderingAsync(emailSpec.HtmlBody, request.Data, cancellationToken);
            
            // Step 3: Create the email message with rendered content
            logger.LogDebug("Creating email message for notification type {NotificationType}", request.Type);
            var emailMessage = CreateEmailMessage(emailSpec, htmlBody);
            
            // Step 4: Add recipients to the email message
            logger.LogDebug("Adding recipients for email specification {Id}", emailSpec.Id);
            AddRecipients(emailMessage, emailSpec);
            
            // Step 5: Add attachments if any
            if (request.Attachments.Count > 0)
            {
                logger.LogDebug("Adding {AttachmentCount} attachments to email message", request.Attachments.Count);
                emailMessage = AddAttachments(emailMessage, request.Attachments);
            }
            
            // Check if we have any recipients
            if (emailMessage.To.Count == 0 && emailMessage.Cc.Count == 0 && emailMessage.Bcc.Count == 0)
            {
                logger.LogWarning("No recipients found for notification type {NotificationType}", request.Type);
                return false;
            }
            
            // Step 6: Send the email
            logger.LogDebug("Sending email for notification type {NotificationType}", request.Type);
            await emailSender.SendEmailAsync(emailMessage, cancellationToken);
            
            logger.LogInformation(
                "Successfully sent notification for type {NotificationType} to {RecipientCount} recipients",
                request.Type,
                emailMessage.To.Count + emailMessage.Cc.Count + emailMessage.Bcc.Count);
            return true;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error processing notification for type {NotificationType}", request.Type);
            return false;
        }
    }
    
    /// <summary>
    /// Retrieves the email specification for the given notification type
    /// </summary>
    private async Task<EmailSpecification?> GetEmailSpecificationAsync(int notificationType, CancellationToken cancellationToken)
    {
        logger.LogDebug("Retrieving email specification for notification type {NotificationType}", notificationType);
        return await emailSpecificationRepository.GetByNotificationTypeAsync((NotificationType)notificationType, cancellationToken);
    }
    
    /// <summary>
    /// Adds recipients to the email message from the email specification
    /// </summary>
    private void AddRecipients(EmailMessage emailMessage, EmailSpecification emailSpec)
    {
        foreach (var group in emailSpec.RecipientGroups)
        {
            foreach (var recipient in group.Recipients)
            {
                var mailAddress = new MailAddress(recipient.EmailAddress, recipient.DisplayName);
                switch (recipient.Type)
                {
                    case RecipientType.To:
                        emailMessage.To.Add(mailAddress);
                        break;
                    case RecipientType.Cc:
                        emailMessage.Cc.Add(mailAddress);
                        break;
                    case RecipientType.Bcc:
                        emailMessage.Bcc.Add(mailAddress);
                        break;
                }
            }
        }
        
        logger.LogDebug("Added {RecipientCount} recipients to email message", 
            emailMessage.To.Count + emailMessage.Cc.Count + emailMessage.Bcc.Count);
    }

    /// <summary>
    /// Adds attachments to the email message from the notification request
    /// </summary>
    /// <returns>A new EmailMessage with the attachments added</returns>
    private EmailMessage AddAttachments(EmailMessage emailMessage, IReadOnlyCollection<IAttachment> attachments)
    {
        var emailAttachments = new List<EmailAttachment>();
        
        foreach (var attachment in attachments)
        {
            emailAttachments.Add(new EmailAttachment
            {
                FileName = attachment.FileName,
                Content = new ReadOnlyMemory<byte>(attachment.Content),
                ContentType = attachment.ContentType,
                IsInline = attachment.IsInline,
                ContentId = attachment.ContentId
            });
        }
        
        // Create a new EmailMessage with attachments using the with-expression
        // This is necessary because Attachments is an init-only property
        var messageWithAttachments = emailMessage with
        {
            Attachments = emailAttachments
        };
        
        logger.LogDebug("Added {AttachmentCount} attachments to email message", attachments.Count);
        
        return messageWithAttachments;
    }

    /// <summary>
    /// Creates an email message from the email specification and rendered content
    /// </summary>
    private static EmailMessage CreateEmailMessage(EmailSpecification emailSpec, string htmlBody, string? textBody = null) =>
        new()
        {
            From = new MailAddress(emailSpec.FromAddress, emailSpec.FromName),
            ReplyTo = emailSpec.ReplyToAddress != null ? new MailAddress(emailSpec.ReplyToAddress) : null,
            Subject = emailSpec.Subject,
            HtmlBody = htmlBody,
            TextBody = textBody ?? emailSpec.TextBody,
            Priority = emailSpec.Priority,
            To = new List<MailAddress>(),
            Cc = new List<MailAddress>(),
            Bcc = new List<MailAddress>()
        };
}