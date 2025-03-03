using EmailNotifications.Application.Enums;
using EmailNotifications.Domain.Entities;
using EmailNotifications.Infrastructure.Interfaces;
using EmailNotifications.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmailNotifications.Infrastructure.Repositories;

/// <summary>
/// Repository for email specifications
/// </summary>
public class EmailSpecificationRepository : IEmailSpecificationRepository
{
    private readonly NotificationDbContext _dbContext;
    private readonly ILogger<EmailSpecificationRepository> _logger;

    public EmailSpecificationRepository(
        NotificationDbContext dbContext,
        ILogger<EmailSpecificationRepository> logger)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<EmailSpecification?> GetByNotificationTypeIdAsync(int notificationTypeId,
        CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving email specification for notification type ID: {NotificationTypeId}", notificationTypeId);

            var emailSpec = await _dbContext.EmailSpecifications
                .Include(e => e.RecipientGroups)
                .ThenInclude(g => g.Recipients)
                .FirstOrDefaultAsync(e => e.NotificationTypeId == notificationTypeId, cancellationToken);

            if (emailSpec == null)
            {
                _logger.LogWarning("Email specification not found for notification type ID: {NotificationTypeId}", notificationTypeId);
                return null;
            }

            _logger.LogDebug("Successfully retrieved email specification {SpecificationName} (ID: {SpecificationId}) for notification type ID: {NotificationTypeId}",
                emailSpec.Name, emailSpec.Id, notificationTypeId);

            return emailSpec;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving email specification for notification type ID: {NotificationTypeId}", notificationTypeId);
            throw;
        }
    }
}