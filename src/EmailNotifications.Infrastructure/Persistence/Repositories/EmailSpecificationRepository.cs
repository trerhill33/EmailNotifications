using EmailNotifications.Domain.Entities;
using EmailNotifications.Domain.Enums;
using EmailNotifications.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmailNotifications.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository implementation for email specifications
/// </summary>
public class EmailSpecificationRepository(
    NotificationDbContext context,
    ILogger<EmailSpecificationRepository> logger)
    : IEmailSpecificationRepository
{
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

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
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

    public async Task<EmailSpecification> AddRecipientGroupAsync(Guid specificationId, EmailRecipientGroup group, CancellationToken cancellationToken = default)
    {
        try
        {
            var specification = await context.EmailSpecifications
                .Include(x => x.RecipientGroups)
                .FirstOrDefaultAsync(x => x.Id == specificationId, cancellationToken);

            if (specification == null)
            {
                throw new InvalidOperationException($"Email specification with ID {specificationId} not found");
            }

            specification.RecipientGroups.Add(group);
            await context.SaveChangesAsync(cancellationToken);
            return specification;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error adding recipient group to email specification {SpecificationId}", specificationId);
            throw;
        }
    }

    public async Task<EmailSpecification> UpdateRecipientGroupAsync(Guid specificationId, EmailRecipientGroup group, CancellationToken cancellationToken = default)
    {
        try
        {
            var specification = await context.EmailSpecifications
                .Include(x => x.RecipientGroups)
                .FirstOrDefaultAsync(x => x.Id == specificationId, cancellationToken);

            if (specification == null)
            {
                throw new InvalidOperationException($"Email specification with ID {specificationId} not found");
            }

            var existingGroup = specification.RecipientGroups.FirstOrDefault(g => g.Id == group.Id);
            if (existingGroup == null)
            {
                throw new InvalidOperationException($"Recipient group with ID {group.Id} not found in specification {specificationId}");
            }

            // Update group properties
            existingGroup.Name = group.Name;
            existingGroup.Description = group.Description;
            existingGroup.LastModifiedAt = DateTime.UtcNow;

            await context.SaveChangesAsync(cancellationToken);
            return specification;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating recipient group {GroupId} in email specification {SpecificationId}", group.Id, specificationId);
            throw;
        }
    }

    public async Task<EmailSpecification> DeleteRecipientGroupAsync(Guid specificationId, Guid groupId, CancellationToken cancellationToken = default)
    {
        try
        {
            var specification = await context.EmailSpecifications
                .Include(x => x.RecipientGroups)
                .FirstOrDefaultAsync(x => x.Id == specificationId, cancellationToken);

            if (specification == null)
            {
                throw new InvalidOperationException($"Email specification with ID {specificationId} not found");
            }

            var group = specification.RecipientGroups.FirstOrDefault(g => g.Id == groupId);
            if (group == null)
            {
                throw new InvalidOperationException($"Recipient group with ID {groupId} not found in specification {specificationId}");
            }

            specification.RecipientGroups.Remove(group);
            await context.SaveChangesAsync(cancellationToken);
            return specification;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting recipient group {GroupId} from email specification {SpecificationId}", groupId, specificationId);
            throw;
        }
    }
} 