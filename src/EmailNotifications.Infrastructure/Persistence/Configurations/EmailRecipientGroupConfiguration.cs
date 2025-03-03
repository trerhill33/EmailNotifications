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

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired();

        builder.Property(e => e.Description);

        builder.Property(e => e.EmailSpecificationId)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.CreatedBy);

        builder.Property(e => e.LastModifiedAt);

        builder.Property(e => e.LastModifiedBy);

        // Relationship with EmailSpecification
        builder.HasOne(e => e.EmailSpecification)
            .WithMany(e => e.RecipientGroups)
            .HasForeignKey(e => e.EmailSpecificationId)
            .OnDelete(DeleteBehavior.Cascade);

        // Indexes for better query performance
        builder.HasIndex(e => new { e.EmailSpecificationId, e.Name })
            .IsUnique();
    }
}