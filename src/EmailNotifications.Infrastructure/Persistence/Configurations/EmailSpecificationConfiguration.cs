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
        
        builder.Property(e => e.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.NotificationType)
            .IsRequired();

        builder.Property(e => e.Subject)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(e => e.HtmlBody)
            .IsRequired();

        builder.Property(e => e.TextBody)
            .HasMaxLength(4000);

        builder.Property(e => e.FromAddress)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.FromName)
            .HasMaxLength(100);

        builder.Property(e => e.ReplyToAddress)
            .HasMaxLength(255);

        builder.Property(e => e.Priority)
            .IsRequired()
            .HasDefaultValue(3);

        builder.Property(e => e.IsActive)
            .IsRequired()
            .HasDefaultValue(true);

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.CreatedBy);

        builder.Property(e => e.LastModifiedAt)
            .IsRequired();

        builder.Property(e => e.LastModifiedBy);

        // Configure one-to-many relationship with EmailRecipientGroup
        builder.HasMany(e => e.RecipientGroups)
            .WithOne(g => g.EmailSpecification)
            .HasForeignKey(g => g.EmailSpecificationId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes for better query performance
        builder.HasIndex(e => e.Name)
            .IsUnique();

        builder.HasIndex(e => e.IsActive);

        // Unique constraint for NotificationType
        builder.HasIndex(e => e.NotificationType)
            .IsUnique();
    }
}