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

        builder.HasKey(r => r.Id);
        
        builder.Property(r => r.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(r => r.EmailAddress)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(r => r.DisplayName)
            .HasMaxLength(100);

        builder.Property(r => r.Type)
            .IsRequired();

        builder.Property(r => r.EmailRecipientGroupId)
            .IsRequired();

        builder.Property(r => r.CreatedAt)
            .IsRequired();

        builder.Property(r => r.CreatedBy);

        builder.Property(r => r.LastModifiedAt)
            .IsRequired();

        builder.Property(r => r.LastModifiedBy);

        // Configure many-to-one relationship with EmailRecipientGroup
        builder.HasOne(r => r.EmailRecipientGroup)
            .WithMany(g => g.Recipients)
            .HasForeignKey(r => r.EmailRecipientGroupId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes for better query performance
        builder.HasIndex(e => new { e.EmailRecipientGroupId, e.EmailAddress, e.Type })
            .IsUnique();

        builder.HasIndex(e => e.EmailAddress);
    }
}