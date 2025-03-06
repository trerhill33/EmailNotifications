using EmailNotifications.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmailNotifications.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuration for the <see cref="EmailRecipientGroup"/> entity.
/// </summary>
public class EmailRecipientGroupConfiguration : IEntityTypeConfiguration<EmailRecipientGroup>
{
    /// <summary>
    /// Configures the entity.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<EmailRecipientGroup> builder)
    {
        builder.ToTable("EmailRecipientGroups");

        builder.HasKey(g => g.Id);
        
        builder.Property(g => g.Id)
            .IsRequired()
            .ValueGeneratedOnAdd();

        builder.Property(g => g.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(g => g.Description);

        builder.Property(g => g.EmailSpecificationId)
            .IsRequired();

        builder.Property(g => g.CreatedAt)
            .IsRequired();

        builder.Property(g => g.CreatedBy);

        builder.Property(g => g.LastModifiedAt)
            .IsRequired();

        builder.Property(g => g.LastModifiedBy);

        // Configure one-to-many relationship with EmailRecipient
        builder.HasMany(g => g.Recipients)
            .WithOne(r => r.EmailRecipientGroup)
            .HasForeignKey(r => r.EmailRecipientGroupId)
            .OnDelete(DeleteBehavior.Cascade);

        // Configure many-to-one relationship with EmailSpecification
        builder.HasOne(g => g.EmailSpecification)
            .WithMany(s => s.RecipientGroups)
            .HasForeignKey(g => g.EmailSpecificationId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes for better query performance
        builder.HasIndex(e => new { e.EmailSpecificationId, e.Name })
            .IsUnique();
    }
}