using EmailNotifications.Domain.Entities;
using EmailNotifications.Domain.Enums;
using EmailNotifications.Infrastructure.Interfaces;
using EmailNotifications.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmailNotifications.Infrastructure.Repositories;

/// <summary>
/// Repository for managing email specifications
/// </summary>
public class EmailSpecificationRepository : IEmailSpecificationRepository
{
    private readonly NotificationDbContext _context;
    private readonly ILogger<EmailSpecificationRepository> _logger;

    public EmailSpecificationRepository(NotificationDbContext context, ILogger<EmailSpecificationRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// Gets an email specification by notification type
    /// </summary>
    /// <param name="notificationType">The notification type to find the specification for</param>
    /// <param name="cancellationToken">The cancellation token</param>
    /// <returns>The email specification if found, null otherwise</returns>
    public async Task<EmailSpecification?> GetByNotificationTypeAsync(NotificationType notificationType, CancellationToken cancellationToken = default)
    {
        try
        {
            _logger.LogDebug("Retrieving email specification for notification type: {NotificationType}", notificationType);

            var emailSpec = await _context.EmailSpecifications
                .Include(e => e.RecipientGroups)
                .FirstOrDefaultAsync(e => e.NotificationType == notificationType, cancellationToken);

            if (emailSpec == null)
            {
                _logger.LogWarning("Email specification not found for notification type: {NotificationType}", notificationType);
                return null;
            }

            _logger.LogDebug("Retrieved email specification {SpecificationName} (ID: {SpecificationId}) for notification type: {NotificationType}",
                emailSpec.Name, emailSpec.Id, notificationType);

            return emailSpec;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving email specification for notification type: {NotificationType}", notificationType);
            throw;
        }
    }
}