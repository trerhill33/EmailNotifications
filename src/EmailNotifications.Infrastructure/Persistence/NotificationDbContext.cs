using EmailNotifications.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace EmailNotifications.Infrastructure.Persistence;

/// <summary>
/// Database context for email notification entities with schema separation
/// </summary>
public class NotificationDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="NotificationDbContext"/> class.
    /// </summary>
    /// <param name="options">The options to be used by the DbContext.</param>
    public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the email specifications.
    /// </summary>
    public DbSet<EmailSpecification> EmailSpecifications => Set<EmailSpecification>();

    /// <summary>
    /// Gets or sets the email recipient groups.
    /// </summary>
    public DbSet<EmailRecipientGroup> EmailRecipientGroups => Set<EmailRecipientGroup>();

    /// <summary>
    /// Gets or sets the email recipients.
    /// </summary>
    public DbSet<EmailRecipient> EmailRecipients => Set<EmailRecipient>();

    /// <summary>
    /// Gets or sets the email logs.
    /// </summary>
    public DbSet<EmailLog> EmailLogs => Set<EmailLog>();

    /// <summary>
    /// Configures the model that was discovered by convention from the entity types.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Set the default schema for all entities
        modelBuilder.HasDefaultSchema("notification");

        // Apply configurations from the current assembly
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}