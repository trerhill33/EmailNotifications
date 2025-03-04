using EmailNotifications.Domain.Entities;
using EmailNotifications.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmailNotifications.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuration for the EmailSpecification entity
/// </summary>
public class EmailSpecificationConfiguration : IEntityTypeConfiguration<EmailSpecification>
{
    /// <summary>
    /// Configures the entity
    /// </summary>
    /// <param name="builder">The entity type builder</param>
    public void Configure(EntityTypeBuilder<EmailSpecification> builder)
    {
        builder.ToTable("EmailSpecifications");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.NotificationType)
            .IsRequired()
            .HasConversion<int>();

        builder.Property(e => e.Subject)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.HtmlBody)
            .IsRequired();

        builder.Property(e => e.TextBody)
            .HasMaxLength(4000);

        builder.Property(e => e.FromAddress)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(e => e.FromName)
            .HasMaxLength(100);

        builder.Property(e => e.ReplyToAddress)
            .HasMaxLength(256);

        builder.Property(e => e.Priority)
            .IsRequired()
            .HasDefaultValue(3);

        builder.Property(e => e.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.CreatedBy);

        builder.Property(e => e.LastModifiedAt);

        builder.Property(e => e.LastModifiedBy);

        // Indexes for better query performance
        builder.HasIndex(e => e.Name)
            .IsUnique();

        builder.HasIndex(e => e.IsActive);

        // Create an index on NotificationType for faster lookups
        builder.HasIndex(e => e.NotificationType);
    }
}