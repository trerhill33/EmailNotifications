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
                        <h1>Welcome to Our Platform!</h1>
                        <p>Dear {{ UserName }},</p>
                        <p>Thank you for joining our platform. We're excited to have you on board!</p>
                        <p>Your account has been successfully created.</p>
                        <p>Best regards,<br>The Team</p>",
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
                        <h1>Password Reset Request</h1>
                        <p>Dear {{ UserName }},</p>
                        <p>We received a request to reset your password. Click the link below to proceed:</p>
                        <p><a href=""{{ ResetLink }}"">Reset Password</a></p>
                        <p>If you didn't request this, please ignore this email.</p>
                        <p>Best regards,<br>The Team</p>",
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
                        <h1>Welcome to Our Community!</h1>
                        <p>Dear {{ UserName }},</p>
                        <p>Welcome to our community! We're thrilled to have you join us.</p>
                        <p>Here are some things you can do to get started:</p>
                        <ul>
                            <li>Complete your profile</li>
                            <li>Explore our features</li>
                            <li>Connect with other members</li>
                        </ul>
                        <p>If you have any questions, feel free to reach out to our support team.</p>
                        <p>Best regards,<br>The Team</p>",
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