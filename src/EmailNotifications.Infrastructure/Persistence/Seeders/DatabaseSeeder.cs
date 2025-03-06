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
                    NotificationType = NotificationType.FedExWeeklyDetailChargesSummary,
                    Name = "FedEx Weekly Detail Charges Summary",
                    Subject = "FedEx Weekly Detail Charges Summary Report",
                    HtmlBody = @"
                        <div class='content'>
                            <h1>{{ report_title }}</h1>
                            <p>Date Range: {{ date_range }}</p>
                            <p>Total Shipments: {{ total_shipments }}</p>
                            <p>Total Cost: ${{ total_cost }}</p>
                            <p>Please find the detailed charges report attached to this email.</p>
                            <p>Best regards,<br>The Logistics Team</p>
                        </div>",
                    TextBody = @"
                        {{ report_title }}

                        Date Range: {{ date_range }}
                        Total Shipments: {{ total_shipments }}
                        Total Cost: ${{ total_cost }}

                        Please find the detailed charges report attached to this email.

                        Best regards,
                        The Logistics Team",
                    FromAddress = "logistics@example.com",
                    FromName = "FedEx Reporting System",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new EmailSpecification
                {
                    NotificationType = NotificationType.FedExRemittanceSummary,
                    Name = "FedEx Remittance Summary",
                    Subject = "FedEx Remittance Summary Report",
                    HtmlBody = @"
                        <div class='content'>
                            <h1>{{ report_title }}</h1>
                            <p>Date Range: {{ date_range }}</p>
                            <p>Total Remittance: ${{ total_remittance }}</p>
                            <p>Please find the remittance summary attached to this email.</p>
                            <p>Best regards,<br>The Logistics Team</p>
                        </div>",
                    TextBody = @"
                        {{ report_title }}

                        Date Range: {{ date_range }}
                        Total Remittance: ${{ total_remittance }}

                        Please find the remittance summary attached to this email.

                        Best regards,
                        The Logistics Team",
                    FromAddress = "logistics@example.com",
                    FromName = "FedEx Reporting System",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new EmailSpecification
                {
                    NotificationType = NotificationType.FedExRemittanceDetails,
                    Name = "FedEx Remittance Details",
                    Subject = "FedEx Remittance Details Report",
                    HtmlBody = @"
                        <div class='content'>
                            <h1>{{ report_title }}</h1>
                            <p>Date Range: {{ date_range }}</p>
                            <p>Total Remittance: ${{ total_remittance }}</p>
                            <p>Please find the detailed remittance report attached to this email.</p>
                            <p>Best regards,<br>The Logistics Team</p>
                        </div>",
                    TextBody = @"
                        {{ report_title }}

                        Date Range: {{ date_range }}
                        Total Remittance: ${{ total_remittance }}

                        Please find the detailed remittance report attached to this email.

                        Best regards,
                        The Logistics Team",
                    FromAddress = "logistics@example.com",
                    FromName = "FedEx Reporting System",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new EmailSpecification
                {
                    NotificationType = NotificationType.FedExFileReceipt,
                    Name = "FedEx File Receipt",
                    Subject = "FedEx File Received and Processed",
                    HtmlBody = @"
                        <div class='content'>
                            <h1>FedEx File Processing Complete</h1>
                            <p>Dear Logistics Team,</p>
                            <p>The FedEx file has been successfully received and processed.</p>
                            <p>File Details:</p>
                            <ul>
                                <li>File Name: {{ file_name }}</li>
                                <li>Received Date: {{ received_date }}</li>
                                <li>Processed Date: {{ processed_date }}</li>
                            </ul>
                            <p>Best regards,<br>The System</p>
                        </div>",
                    TextBody = @"
                        FedEx File Processing Complete

                        Dear Logistics Team,

                        The FedEx file has been successfully received and processed.

                        File Details:
                        - File Name: {{ file_name }}
                        - Received Date: {{ received_date }}
                        - Processed Date: {{ processed_date }}

                        Best regards,
                        The System",
                    FromAddress = "system@example.com",
                    FromName = "FedEx Processing System",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new EmailSpecification
                {
                    NotificationType = NotificationType.FedExFileMissing,
                    Name = "FedEx File Missing",
                    Subject = "FedEx File Not Received",
                    HtmlBody = @"
                        <div class='content'>
                            <h1>FedEx File Missing Alert</h1>
                            <p>Dear Logistics Team,</p>
                            <p>The expected FedEx file has not been received.</p>
                            <p>Expected File Details:</p>
                            <ul>
                                <li>Expected Date: {{ expected_date }}</li>
                                <li>File Type: {{ file_type }}</li>
                            </ul>
                            <p>Please investigate this issue.</p>
                            <p>Best regards,<br>The System</p>
                        </div>",
                    TextBody = @"
                        FedEx File Missing Alert

                        Dear Logistics Team,

                        The expected FedEx file has not been received.

                        Expected File Details:
                        - Expected Date: {{ expected_date }}
                        - File Type: {{ file_type }}

                        Please investigate this issue.

                        Best regards,
                        The System",
                    FromAddress = "system@example.com",
                    FromName = "FedEx Processing System",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new EmailSpecification
                {
                    NotificationType = NotificationType.DailyReassignedTrackingNumbers,
                    Name = "Daily Reassigned Tracking Numbers",
                    Subject = "Daily Reassigned Tracking Numbers Report",
                    HtmlBody = @"
                        <div class='content'>
                            <h1>{{ report_title }}</h1>
                            <p>Date: {{ report_date }}</p>
                            <p>Total Reassigned: {{ total_reassigned }}</p>
                            <p>Please find the detailed report attached to this email.</p>
                            <p>Best regards,<br>The Logistics Team</p>
                        </div>",
                    TextBody = @"
                        {{ report_title }}

                        Date: {{ report_date }}
                        Total Reassigned: {{ total_reassigned }}

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
                    NotificationType = NotificationType.DelayedInvoicesReport,
                    Name = "Delayed Invoices Report",
                    Subject = "Delayed Invoices Report",
                    HtmlBody = @"
                        <div class='content'>
                            <h1>{{ report_title }}</h1>
                            <p>Date: {{ report_date }}</p>
                            <p>Total Delayed Invoices: {{ total_delayed }}</p>
                            <p>Please find the detailed report attached to this email.</p>
                            <p>Best regards,<br>The Finance Team</p>
                        </div>",
                    TextBody = @"
                        {{ report_title }}

                        Date: {{ report_date }}
                        Total Delayed Invoices: {{ total_delayed }}

                        Please find the detailed report attached to this email.

                        Best regards,
                        The Finance Team",
                    FromAddress = "finance@example.com",
                    FromName = "Finance Reporting System",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new EmailSpecification
                {
                    NotificationType = NotificationType.PendingApprovalNotification,
                    Name = "Pending Approval Notification",
                    Subject = "Pending Approval Required",
                    HtmlBody = @"
                        <div class='content'>
                            <h1>Pending Approval Required</h1>
                            <p>Dear {{ approver_name }},</p>
                            <p>You have {{ pending_count }} items awaiting your approval.</p>
                            <p>Please review and process these items at your earliest convenience.</p>
                            <p>Best regards,<br>The System</p>
                        </div>",
                    TextBody = @"
                        Pending Approval Required

                        Dear {{ approver_name }},

                        You have {{ pending_count }} items awaiting your approval.

                        Please review and process these items at your earliest convenience.

                        Best regards,
                        The System",
                    FromAddress = "system@example.com",
                    FromName = "Approval System",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow
                },
                new EmailSpecification
                {
                    NotificationType = NotificationType.WeeklyTrackingByBusinessUnit,
                    Name = "Weekly Tracking By Business Unit",
                    Subject = "Weekly Tracking Numbers By Business Unit Report",
                    HtmlBody = @"
                        <div class='content'>
                            <h1>{{ report_title }}</h1>
                            <p>Date Range: {{ date_range }}</p>
                            <p>Total Business Units: {{ total_business_units }}</p>
                            <p>Please find the detailed report attached to this email.</p>
                            <p>Best regards,<br>The Logistics Team</p>
                        </div>",
                    TextBody = @"
                        {{ report_title }}

                        Date Range: {{ date_range }}
                        Total Business Units: {{ total_business_units }}

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
                    NotificationType = NotificationType.InvalidEmployeeIdSummary,
                    Name = "Invalid Employee ID Summary",
                    Subject = "Invalid Employee ID Summary Report",
                    HtmlBody = @"
                        <div class='content'>
                            <h1>{{ report_title }}</h1>
                            <p>Date: {{ report_date }}</p>
                            <p>Total Invalid IDs: {{ total_invalid_ids }}</p>
                            <p>Please find the detailed report attached to this email.</p>
                            <p>Best regards,<br>The HR Team</p>
                        </div>",
                    TextBody = @"
                        {{ report_title }}

                        Date: {{ report_date }}
                        Total Invalid IDs: {{ total_invalid_ids }}

                        Please find the detailed report attached to this email.

                        Best regards,
                        The HR Team",
                    FromAddress = "hr@example.com",
                    FromName = "HR Reporting System",
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
} 