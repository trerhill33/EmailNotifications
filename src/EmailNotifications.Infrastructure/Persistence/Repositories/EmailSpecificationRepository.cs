using EmailNotifications.Domain.Entities;
using EmailNotifications.Domain.Enums;
using EmailNotifications.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmailNotifications.Infrastructure.Persistence.Repositories;

public class EmailSpecificationRepository(
    NotificationDbContext context,
    ILogger<EmailSpecificationRepository> logger)
    : IEmailSpecificationRepository
{
    public async Task<IEnumerable<EmailSpecification>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            return await context.EmailSpecifications
                .Include(x => x.RecipientGroups)
                    .ThenInclude(g => g.Recipients)
                .OrderBy(x => x.Name)
                .ToListAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving all email specifications");
            throw;
        }
    }

    public async Task<EmailSpecification?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            return await context.EmailSpecifications
                .Include(x => x.RecipientGroups)
                    .ThenInclude(g => g.Recipients)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving email specification with ID {Id}", id);
            throw;
        }
    }

    public async Task<EmailSpecification?> GetByNotificationTypeAsync(NotificationType notificationType, CancellationToken cancellationToken = default)
    {
        try
        {
            return await context.EmailSpecifications
                .Include(x => x.RecipientGroups)
                    .ThenInclude(g => g.Recipients)
                .FirstOrDefaultAsync(x => x.NotificationType == notificationType, cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving email specification for notification type {NotificationType}", notificationType);
            throw;
        }
    }

    public async Task<EmailSpecification> AddAsync(EmailSpecification specification, CancellationToken cancellationToken = default)
    {
        try
        {
            await context.EmailSpecifications.AddAsync(specification, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);
            return specification;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error adding email specification for notification type {NotificationType}", specification.NotificationType);
            throw;
        }
    }

    public async Task<EmailSpecification> UpdateAsync(EmailSpecification specification, CancellationToken cancellationToken = default)
    {
        try
        {
            var existing = await context.EmailSpecifications
                .Include(x => x.RecipientGroups)
                    .ThenInclude(g => g.Recipients)
                .FirstOrDefaultAsync(x => x.Id == specification.Id, cancellationToken);

            if (existing == null)
            {
                throw new InvalidOperationException($"Email specification with ID {specification.Id} not found");
            }

            // Update basic properties
            existing.Name = specification.Name;
            existing.Subject = specification.Subject;
            existing.HtmlBody = specification.HtmlBody;
            existing.TextBody = specification.TextBody;
            existing.FromAddress = specification.FromAddress;
            existing.FromName = specification.FromName;
            existing.ReplyToAddress = specification.ReplyToAddress;
            existing.Priority = specification.Priority;
            existing.IsActive = specification.IsActive;
            existing.LastModifiedAt = DateTime.UtcNow;

            await context.SaveChangesAsync(cancellationToken);
            return existing;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating email specification with ID {Id}", specification.Id);
            throw;
        }
    }

    public async Task DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        try
        {
            var specification = await context.EmailSpecifications
                .Include(x => x.RecipientGroups)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

            if (specification == null)
            {
                throw new InvalidOperationException($"Email specification with ID {id} not found");
            }

            context.EmailSpecifications.Remove(specification);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting email specification with ID {Id}", id);
            throw;
        }
    }
    
    public async Task<IEnumerable<EmailRecipient>> GetRecipientsAsync(int specificationId, CancellationToken cancellationToken = default)
    {
        try
        {
            var specification = await context.EmailSpecifications
                .Include(x => x.RecipientGroups)
                    .ThenInclude(g => g.Recipients)
                .FirstOrDefaultAsync(x => x.Id == specificationId, cancellationToken);

            if (specification == null)
            {
                throw new InvalidOperationException($"Email specification with ID {specificationId} not found");
            }

            var recipients = new List<EmailRecipient>();
            foreach (var group in specification.RecipientGroups)
            {
                recipients.AddRange(group.Recipients);
            }

            return recipients;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving recipients for specification {SpecificationId}", specificationId);
            throw;
        }
    }

    public async Task<EmailRecipient> AddRecipientAsync(int specificationId, EmailRecipient recipient, CancellationToken cancellationToken = default)
    {
        try
        {
            var specification = await context.EmailSpecifications
                .Include(x => x.RecipientGroups)
                    .ThenInclude(g => g.Recipients)
                .FirstOrDefaultAsync(x => x.Id == specificationId, cancellationToken);

            if (specification == null)
            {
                throw new InvalidOperationException($"Email specification with ID {specificationId} not found");
            }

            var group = specification.RecipientGroups.FirstOrDefault();
            if (group == null)
            {
                group = new EmailRecipientGroup
                {
                    Name = "Default",
                    CreatedAt = DateTime.UtcNow,
                    LastModifiedAt = DateTime.UtcNow,
                    Recipients = new List<EmailRecipient>()
                };
                specification.RecipientGroups.Add(group);
            }

            // Check for duplicate email
            if (group.Recipients.Any(r => r.EmailAddress.Equals(recipient.EmailAddress, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException($"Recipient with email {recipient.EmailAddress} already exists in the group");
            }

            recipient.CreatedAt = DateTime.UtcNow;
            recipient.LastModifiedAt = DateTime.UtcNow;
            group.Recipients.Add(recipient);

            await context.SaveChangesAsync(cancellationToken);
            return recipient;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error adding recipient to specification {SpecificationId}", specificationId);
            throw;
        }
    }

    public async Task<EmailRecipient> UpdateRecipientAsync(int specificationId, EmailRecipient recipient, CancellationToken cancellationToken = default)
    {
        try
        {
            var specification = await context.EmailSpecifications
                .Include(x => x.RecipientGroups)
                    .ThenInclude(g => g.Recipients)
                .FirstOrDefaultAsync(x => x.Id == specificationId, cancellationToken);

            if (specification == null)
            {
                throw new InvalidOperationException($"Email specification with ID {specificationId} not found");
            }

            EmailRecipient? existingRecipient = null;
            foreach (var group in specification.RecipientGroups)
            {
                existingRecipient = group.Recipients.FirstOrDefault(r => r.Id == recipient.Id);
                if (existingRecipient != null)
                    break;
            }

            if (existingRecipient == null)
            {
                throw new InvalidOperationException($"Recipient with ID {recipient.Id} not found in specification {specificationId}");
            }

            // Update recipient properties
            existingRecipient.EmailAddress = recipient.EmailAddress;
            existingRecipient.DisplayName = recipient.DisplayName;
            existingRecipient.Type = recipient.Type;
            existingRecipient.LastModifiedAt = DateTime.UtcNow;

            await context.SaveChangesAsync(cancellationToken);
            return existingRecipient;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating recipient in specification {SpecificationId}", specificationId);
            throw;
        }
    }

    public async Task DeleteRecipientAsync(int specificationId, string email, CancellationToken cancellationToken = default)
    {
        try
        {
            var specification = await context.EmailSpecifications
                .Include(x => x.RecipientGroups)
                    .ThenInclude(g => g.Recipients)
                .FirstOrDefaultAsync(x => x.Id == specificationId, cancellationToken);

            if (specification == null)
            {
                throw new InvalidOperationException($"Email specification with ID {specificationId} not found");
            }

            EmailRecipient? recipientToRemove = null;
            EmailRecipientGroup? groupWithRecipient = null;

            foreach (var group in specification.RecipientGroups)
            {
                recipientToRemove = group.Recipients.FirstOrDefault(r => r.EmailAddress.Equals(email, StringComparison.OrdinalIgnoreCase));
                if (recipientToRemove != null)
                {
                    groupWithRecipient = group;
                    break;
                }
            }

            if (recipientToRemove == null || groupWithRecipient == null)
            {
                throw new InvalidOperationException($"Recipient with email {email} not found in specification {specificationId}");
            }

            groupWithRecipient.Recipients.Remove(recipientToRemove);
            await context.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting recipient from specification {SpecificationId}", specificationId);
            throw;
        }
    }
} 