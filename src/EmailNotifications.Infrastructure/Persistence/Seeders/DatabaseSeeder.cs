using EmailNotifications.Domain.Entities;
using EmailNotifications.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmailNotifications.Infrastructure.Persistence.Seeders;

public class DatabaseSeeder(NotificationDbContext context, ILogger<DatabaseSeeder> logger)
{
    public async Task SeedAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            logger.LogInformation("Starting database seeding");

            // Check if database has been seeded
            if (await context.EmailSpecifications.AnyAsync(cancellationToken))
            {
                logger.LogInformation("Database has already been seeded");
                return;
            }

            // Seed Email Specifications
            var emailSpecifications = new[]
            {
                new EmailSpecification
                {
                    Id = Guid.NewGuid(),
                    NotificationType = NotificationType.NewUser,
                    Name = "New User Notification",
                    Subject = "Welcome to Our Platform",
                    HtmlBody = @"
                        <div class='content'>
                            <h1>Welcome to Our Platform!</h1>
                            <p>Dear {{ UserName }},</p>
                            <p>Thank you for joining our platform. We're excited to have you on board!</p>
                            <p>Your account has been successfully created.</p>
                            <p>Best regards,<br>The Team</p>
                        </div>",
                    TextBody = @"
                        Welcome to Our Platform!

                        Dear {{ UserName }},

                        Thank you for joining our platform. We're excited to have you on board!

                        Your account has been successfully created.

                        Best regards,
                        The Team",
                    FromAddress = "noreply@example.com",
                    FromName = "Our Platform",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new EmailSpecification
                {
                    Id = Guid.NewGuid(),
                    NotificationType = NotificationType.PasswordReset,
                    Name = "Password Reset Notification",
                    Subject = "Password Reset Request",
                    HtmlBody = @"
                        <div class='content'>
                            <h1>Password Reset Request</h1>
                            <p>Dear {{ UserName }},</p>
                            <p>We received a request to reset your password. Click the button below to proceed:</p>
                            <p style='text-align: center;'>
                                <a href='{{ ResetLink }}' class='button'>Reset Password</a>
                            </p>
                            <p>If you didn't request this, please ignore this email.</p>
                            <p>Best regards,<br>The Team</p>
                        </div>",
                    TextBody = @"
                        Password Reset Request

                        Dear {{ UserName }},

                        We received a request to reset your password. Click the link below to proceed:

                        {{ ResetLink }}

                        If you didn't request this, please ignore this email.

                        Best regards,
                        The Team",
                    FromAddress = "noreply@example.com",
                    FromName = "Our Platform",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new EmailSpecification
                {
                    Id = Guid.NewGuid(),
                    NotificationType = NotificationType.Welcome,
                    Name = "Welcome Email",
                    Subject = "Welcome to Our Community!",
                    HtmlBody = @"
                        <div class='content'>
                            <h1>Welcome to Our Community!</h1>
                            <p>Dear {{ UserName }},</p>
                            <p>Welcome to our community! We're thrilled to have you join us.</p>
                            <p>Here are some things you can do to get started:</p>
                            <ul class='list'>
                                <li>Complete your profile</li>
                                <li>Explore our features</li>
                                <li>Connect with other members</li>
                            </ul>
                            <p>If you have any questions, feel free to reach out to our support team.</p>
                            <p>Best regards,<br>The Team</p>
                        </div>",
                    TextBody = @"
                        Welcome to Our Community!

                        Dear {{ UserName }},

                        Welcome to our community! We're thrilled to have you join us.

                        Here are some things you can do to get started:
                        - Complete your profile
                        - Explore our features
                        - Connect with other members

                        If you have any questions, feel free to reach out to our support team.

                        Best regards,
                        The Team",
                    FromAddress = "noreply@example.com",
                    FromName = "Our Platform",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.EmailSpecifications.AddRangeAsync(emailSpecifications, cancellationToken);

            // Create email groups for each specification
            foreach (var spec in emailSpecifications)
            {
                var adminGroup = new EmailRecipientGroup
                {
                    Id = Guid.NewGuid(),
                    Name = "Administrators",
                    Description = "System administrators",
                    EmailSpecificationId = spec.Id,
                    CreatedAt = DateTime.UtcNow
                };

                var supportGroup = new EmailRecipientGroup
                {
                    Id = Guid.NewGuid(),
                    Name = "Support Team",
                    Description = "Customer support team",
                    EmailSpecificationId = spec.Id,
                    CreatedAt = DateTime.UtcNow
                };

                await context.EmailRecipientGroups.AddRangeAsync(new[] { adminGroup, supportGroup }, cancellationToken);

                // Add recipients to groups
                var adminRecipients = new[]
                {
                    new EmailRecipient
                    {
                        Id = Guid.NewGuid(),
                        EmailAddress = "admin@example.com",
                        DisplayName = "System Administrator",
                        Type = RecipientType.To,
                        EmailRecipientGroupId = adminGroup.Id,
                        CreatedAt = DateTime.UtcNow
                    }
                };

                var supportRecipients = new[]
                {
                    new EmailRecipient
                    {
                        Id = Guid.NewGuid(),
                        EmailAddress = "support@example.com",
                        DisplayName = "Support Team",
                        Type = RecipientType.To,
                        EmailRecipientGroupId = supportGroup.Id,
                        CreatedAt = DateTime.UtcNow
                    }
                };

                await context.EmailRecipients.AddRangeAsync(adminRecipients.Concat(supportRecipients), cancellationToken);
            }

            await context.SaveChangesAsync(cancellationToken);
            logger.LogInformation("Database seeding completed successfully");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error seeding database");
            throw;
        }
    }
} 