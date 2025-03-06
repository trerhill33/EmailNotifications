using EmailNotifications.Domain.Entities;
using EmailNotifications.Domain.Enums;
using EmailNotifications.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmailNotifications.Infrastructure.Persistence.Seeders;

public class DatabaseSeeder(NotificationDbContext context, ILogger<DatabaseSeeder> logger, IEmailSpecificationRepository specificationRepository)
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
                    NotificationType = NotificationType.NewUser,
                    Name = "New User Notification",
                    Subject = "Welcome to Our Platform",
                    HtmlBody = @"
                        <div class='content'>
                            <h1>Welcome to Our Platform!</h1>
                            <p>Dear {{ first_name }} {{ last_name }},</p>
                            <p>Thank you for joining our platform. We're excited to have you on board!</p>
                            <p>Your account has been successfully created.</p>
                            <p>Best regards,<br>The Team</p>
                        </div>",
                    TextBody = @"
                        Welcome to Our Platform!

                        Dear {{ first_name }} {{ last_name }},

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
                    NotificationType = NotificationType.PasswordReset,
                    Name = "Password Reset Notification",
                    Subject = "Password Reset Request",
                    HtmlBody = @"
                        <div class='content'>
                            <h1>Password Reset Request</h1>
                            <p>Dear {{ first_name }} {{ last_name }},</p>
                            <p>We received a request to reset your password. Click the button below to proceed:</p>
                            <p style='text-align: center;'>
                                <a href='{{ reset_link }}' class='button'>Reset Password</a>
                            </p>
                            <p>If you didn't request this, please ignore this email.</p>
                            <p>Best regards,<br>The Team</p>
                        </div>",
                    TextBody = @"
                        Password Reset Request

                        Dear {{ first_name }} {{ last_name }},

                        We received a request to reset your password. Click the link below to proceed:

                        {{ reset_link }}

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
                    NotificationType = NotificationType.Welcome,
                    Name = "Welcome Email",
                    Subject = "Welcome to Our Community!",
                    HtmlBody = @"
                        <div class='content'>
                            <h1>Welcome to Our Community!</h1>
                            <p>Dear {{ first_name }} {{ last_name }},</p>
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

                        Dear {{ first_name }} {{ last_name }},

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
                },
                new EmailSpecification
                {
                    NotificationType = NotificationType.FedExWeeklyChargesSummary,
                    Name = "FedEx Weekly Charges Summary",
                    Subject = "FedEx Weekly Charges Summary Report",
                    HtmlBody = @"
                        <div class='content'>
                            <h1>{{ report_title }}</h1>
                            <p>Date Range: {{ date_range }}</p>
                            <p>Total Shipments: {{ total_shipments }}</p>
                            <p>Total Cost: ${{ total_cost }}</p>
                            <p>Please find the detailed report attached to this email.</p>
                            <p>Best regards,<br>The Logistics Team</p>
                        </div>",
                    TextBody = @"
                        {{ report_title }}

                        Date Range: {{ date_range }}
                        Total Shipments: {{ total_shipments }}
                        Total Cost: ${{ total_cost }}

                        Please find the detailed report attached to this email.

                        Best regards,
                        The Logistics Team",
                    FromAddress = "logistics@example.com",
                    FromName = "FedEx Reporting System",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new EmailSpecification
                {
                    NotificationType = NotificationType.FedExMonthlyDeliveryPerformance,
                    Name = "FedEx Monthly Delivery Performance",
                    Subject = "FedEx Monthly Delivery Performance Report",
                    HtmlBody = @"
                        <div class='content'>
                            <h1>{{ report_title }}</h1>
                            <p>Date Range: {{ date_range }}</p>
                            <p>Total Shipments: {{ total_shipments }}</p>
                            <p>Total Cost: ${{ total_cost }}</p>
                            <p>Please find the detailed performance report attached to this email.</p>
                            <p>Best regards,<br>The Logistics Team</p>
                        </div>",
                    TextBody = @"
                        {{ report_title }}

                        Date Range: {{ date_range }}
                        Total Shipments: {{ total_shipments }}
                        Total Cost: ${{ total_cost }}

                        Please find the detailed performance report attached to this email.

                        Best regards,
                        The Logistics Team",
                    FromAddress = "logistics@example.com",
                    FromName = "FedEx Reporting System",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new EmailSpecification
                {
                    NotificationType = NotificationType.FedExQuarterlyCostAnalysis,
                    Name = "FedEx Quarterly Cost Analysis",
                    Subject = "FedEx Quarterly Cost Analysis Report",
                    HtmlBody = @"
                        <div class='content'>
                            <h1>{{ report_title }}</h1>
                            <p>Date Range: {{ date_range }}</p>
                            <p>Total Shipments: {{ total_shipments }}</p>
                            <p>Total Cost: ${{ total_cost }}</p>
                            <p>Please find the detailed cost analysis report attached to this email.</p>
                            <p>Best regards,<br>The Logistics Team</p>
                        </div>",
                    TextBody = @"
                        {{ report_title }}

                        Date Range: {{ date_range }}
                        Total Shipments: {{ total_shipments }}
                        Total Cost: ${{ total_cost }}

                        Please find the detailed cost analysis report attached to this email.

                        Best regards,
                        The Logistics Team",
                    FromAddress = "logistics@example.com",
                    FromName = "FedEx Reporting System",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                }
            };

            await context.EmailSpecifications.AddRangeAsync(emailSpecifications, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            // Create email groups for each specification
            foreach (var spec in emailSpecifications)
            {
                var adminGroup = new EmailRecipientGroup
                {
                    Name = "Administrators",
                    Description = "System administrators",
                    EmailSpecificationId = spec.Id,
                    CreatedAt = DateTime.UtcNow
                };

                var supportGroup = new EmailRecipientGroup
                {
                    Name = "Support Team",
                    Description = "Customer support team",
                    EmailSpecificationId = spec.Id,
                    CreatedAt = DateTime.UtcNow
                };

                await context.EmailRecipientGroups.AddRangeAsync(new[] { adminGroup, supportGroup }, cancellationToken);
                await context.SaveChangesAsync(cancellationToken);

                // Add recipients to groups
                var adminRecipients = new[]
                {
                    new EmailRecipient
                    {
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

    public async Task SeedWelcomeAndPasswordResetTemplates()
    {
        var welcomeTemplate = new EmailSpecification
        {
            Name = "Welcome Email",
            NotificationType = NotificationType.Welcome,
            Subject = "Welcome to Our Platform!",
            HtmlBody = @"<div class='content'>
                <h1>Welcome to Our Platform!</h1>
                <p>Dear {{ first_name }} {{ last_name }},</p>
                <p>Thank you for joining our platform. We're excited to have you on board!</p>
                <p>Your account has been successfully created on {{ formatted_date }}.</p>
                <p>Best regards,<br>The Team</p>
            </div>",
            TextBody = "Welcome to Our Platform!\n\nDear {{ first_name }} {{ last_name }},\n\nThank you for joining our platform. We're excited to have you on board!\n\nYour account has been successfully created on {{ formatted_date }}.\n\nBest regards,\nThe Team",
            FromAddress = "noreply@example.com",
            FromName = "Our Platform",
            Priority = 3,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System",
            LastModifiedAt = DateTime.UtcNow,
            LastModifiedBy = "System"
        };

        var passwordResetTemplate = new EmailSpecification
        {
            Name = "Password Reset",
            NotificationType = NotificationType.PasswordReset,
            Subject = "Password Reset Request",
            HtmlBody = @"<div class='content'>
                <h1>Password Reset Request</h1>
                <p>Dear {{ first_name }},</p>
                <p>We received a request to reset your password. Your one-time password is: {{ one_time_password }}</p>
                <p>This password will expire at: {{ expiry_time_formatted }}</p>
                <p>If you didn't request this, please ignore this email.</p>
                <p>Best regards,<br>The Team</p>
            </div>",
            TextBody = "Password Reset Request\n\nDear {{ first_name }},\n\nWe received a request to reset your password. Your one-time password is: {{ one_time_password }}\n\nThis password will expire at: {{ expiry_time_formatted }}\n\nIf you didn't request this, please ignore this email.\n\nBest regards,\nThe Team",
            FromAddress = "noreply@example.com",
            FromName = "Our Platform",
            Priority = 1,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System",
            LastModifiedAt = DateTime.UtcNow,
            LastModifiedBy = "System"
        };

        var fedExWeeklyTemplate = new EmailSpecification
        {
            Name = "FedEx Weekly Charges Summary",
            NotificationType = NotificationType.FedExWeeklyChargesSummary,
            Subject = "FedEx Weekly Charges Summary Report",
            HtmlBody = @"<div class='content'>
                <h1>{{ report_title }}</h1>
                <p>Date Range: {{ date_range }}</p>
                <p>Total Shipments: {{ total_shipments }}</p>
                <p>Total Cost: ${{ total_cost }}</p>
                <p>Please find the detailed report attached to this email.</p>
                <p>Best regards,<br>The Logistics Team</p>
            </div>",
            TextBody = "{{ report_title }}\n\nDate Range: {{ date_range }}\nTotal Shipments: {{ total_shipments }}\nTotal Cost: ${{ total_cost }}\n\nPlease find the detailed report attached to this email.\n\nBest regards,\nThe Logistics Team",
            FromAddress = "logistics@example.com",
            FromName = "FedEx Reporting System",
            Priority = 2,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System",
            LastModifiedAt = DateTime.UtcNow,
            LastModifiedBy = "System"
        };

        var fedExMonthlyTemplate = new EmailSpecification
        {
            Name = "FedEx Monthly Delivery Performance",
            NotificationType = NotificationType.FedExMonthlyDeliveryPerformance,
            Subject = "FedEx Monthly Delivery Performance Report",
            HtmlBody = @"<div class='content'>
                <h1>{{ report_title }}</h1>
                <p>Date Range: {{ date_range }}</p>
                <p>Total Shipments: {{ total_shipments }}</p>
                <p>Total Cost: ${{ total_cost }}</p>
                <p>Please find the detailed performance report attached to this email.</p>
                <p>Best regards,<br>The Logistics Team</p>
            </div>",
            TextBody = "{{ report_title }}\n\nDate Range: {{ date_range }}\nTotal Shipments: {{ total_shipments }}\nTotal Cost: ${{ total_cost }}\n\nPlease find the detailed performance report attached to this email.\n\nBest regards,\nThe Logistics Team",
            FromAddress = "logistics@example.com",
            FromName = "FedEx Reporting System",
            Priority = 2,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            CreatedBy = "System",
            LastModifiedAt = DateTime.UtcNow,
            LastModifiedBy = "System"
        };

        // Check if templates already exist
        var existingWelcomeTemplate = await specificationRepository.GetByNotificationTypeAsync(NotificationType.Welcome);
        var existingPasswordResetTemplate = await specificationRepository.GetByNotificationTypeAsync(NotificationType.PasswordReset);
        var existingFedExWeeklyTemplate = await specificationRepository.GetByNotificationTypeAsync(NotificationType.FedExWeeklyChargesSummary);
        var existingFedExMonthlyTemplate = await specificationRepository.GetByNotificationTypeAsync(NotificationType.FedExMonthlyDeliveryPerformance);

        if (existingWelcomeTemplate == null)
        {
            await specificationRepository.AddAsync(welcomeTemplate);
        }

        if (existingPasswordResetTemplate == null)
        {
            await specificationRepository.AddAsync(passwordResetTemplate);
        }

        if (existingFedExWeeklyTemplate == null)
        {
            await specificationRepository.AddAsync(fedExWeeklyTemplate);
        }

        if (existingFedExMonthlyTemplate == null)
        {
            await specificationRepository.AddAsync(fedExMonthlyTemplate);
        }
    }
} 