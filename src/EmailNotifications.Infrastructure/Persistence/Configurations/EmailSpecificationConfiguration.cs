using EmailNotifications.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmailNotifications.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuration for the <see cref="EmailSpecification"/> entity.
/// </summary>
public class EmailSpecificationConfiguration : IEntityTypeConfiguration<EmailSpecification>
{
    /// <summary>
    /// Configures the entity.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<EmailSpecification> builder)
    {
        builder.ToTable("EmailSpecifications");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired();

        builder.Property(e => e.NotificationTypeId)
            .IsRequired();

        builder.Property(e => e.Subject)
            .IsRequired();

        builder.Property(e => e.HtmlBody)
            .IsRequired();

        builder.Property(e => e.TextBody);

        builder.Property(e => e.FromAddress)
            .IsRequired();

        builder.Property(e => e.FromName);

        builder.Property(e => e.ReplyToAddress);

        builder.Property(e => e.Priority)
            .IsRequired();

        builder.Property(e => e.IsActive)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.CreatedBy);

        builder.Property(e => e.LastModifiedAt);

        builder.Property(e => e.LastModifiedBy);

        // Indexes for better query performance
        builder.HasIndex(e => e.Name)
            .IsUnique();

        builder.HasIndex(e => e.IsActive);

        // Create an index on NotificationTypeId for faster lookups
        builder.HasIndex(e => e.NotificationTypeId);
    }
}