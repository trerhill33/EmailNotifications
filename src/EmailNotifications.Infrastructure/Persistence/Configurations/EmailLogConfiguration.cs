using EmailNotifications.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EmailNotifications.Infrastructure.Persistence.Configurations;

/// <summary>
/// Configuration for the <see cref="EmailLog"/> entity.
/// </summary>
public class EmailLogConfiguration : IEntityTypeConfiguration<EmailLog>
{
    /// <summary>
    /// Configures the entity.
    /// </summary>
    /// <param name="builder">The builder to be used to configure the entity.</param>
    public void Configure(EntityTypeBuilder<EmailLog> builder)
    {
        builder.ToTable("EmailLogs");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.EmailSpecificationId)
            .IsRequired();

        builder.Property(e => e.Subject)
            .IsRequired();

        builder.Property(e => e.FromAddress)
            .IsRequired();

        builder.Property(e => e.FromName);

        builder.Property(e => e.ToAddresses)
            .IsRequired();

        builder.Property(e => e.CcAddresses);

        builder.Property(e => e.BccAddresses);

        builder.Property(e => e.Status)
            .IsRequired();

        builder.Property(e => e.SentAt);

        builder.Property(e => e.AttemptCount)
            .IsRequired();

        builder.Property(e => e.NextAttemptAt);

        builder.Property(e => e.ErrorMessage);

        builder.Property(e => e.SerializedModel);

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.Property(e => e.CreatedBy);

        builder.Property(e => e.LastModifiedAt);

        builder.Property(e => e.LastModifiedBy);

        // Relationship with EmailSpecification
        builder.HasOne(e => e.EmailSpecification)
            .WithMany()
            .HasForeignKey(e => e.EmailSpecificationId)
            .OnDelete(DeleteBehavior.Restrict);

        // Indexes for better query performance
        builder.HasIndex(e => e.Status);
        builder.HasIndex(e => e.NextAttemptAt);
        builder.HasIndex(e => e.CreatedAt);
        builder.HasIndex(e => e.SentAt);
    }
}