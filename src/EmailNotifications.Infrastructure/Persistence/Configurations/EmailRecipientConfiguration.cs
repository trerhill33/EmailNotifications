using EmailNotifications.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmailNotifications.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuration for the <see cref="EmailRecipient"/> entity.
/// </summary>
public class EmailRecipientConfiguration : IEntityTypeConfiguration<EmailRecipient>
{
    /// <summary>
    /// Configures the entity.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<EmailRecipient> builder)
    {
        builder.ToTable("EmailRecipients");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.EmailAddress)
            .IsRequired();

        builder.Property(e => e.DisplayName);

        builder.Property(e => e.Type)
            .IsRequired();

        builder.Property(e => e.EmailRecipientGroupId)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.CreatedBy);

        builder.Property(e => e.LastModifiedAt);

        builder.Property(e => e.LastModifiedBy);

        // Relationship with EmailRecipientGroup
        builder.HasOne(e => e.EmailRecipientGroup)
            .WithMany(e => e.Recipients)
            .HasForeignKey(e => e.EmailRecipientGroupId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes for better query performance
        builder.HasIndex(e => new { e.EmailRecipientGroupId, e.EmailAddress, e.Type })
            .IsUnique();

        builder.HasIndex(e => e.EmailAddress);
    }
}