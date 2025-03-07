using EmailNotifications.Domain.Entities;
using EmailNotifications.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace EmailNotifications.Infrastructure.Persistence.Seeders;

public class ReportNotificationsSeeder(NotificationDbContext db, ILogger<ReportNotificationsSeeder> logger)
{
    public void SeedReportNotifications()
    {
        Task.Run(async () =>
        {
            try
            {
                logger.LogInformation("Starting report notifications seeding");

                await SeedFedExWeeklyChargesSummaryAsync();
                await SeedFedExWeeklyDetailChargesSummaryAsync();
                await SeedFedExRemittanceSummaryAsync();
                await SeedFedExRemittanceDetailsAsync();
                await SeedFedExFileReceiptAsync();
                await SeedFedExFileMissingAsync();
                await SeedReassignedTrackingNumbersAsync();
                await SeedDelayedInvoicesAsync();
                await SeedPendingApprovalAsync();
                await SeedTrackingNumbersByBusinessUnitAsync();
                await SeedInvalidEmployeeIdAsync();

                logger.LogInformation("Report notifications seeding completed successfully");
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error seeding report notifications");
            }
        });
    }

    private async Task SeedFedExWeeklyChargesSummaryAsync()
    {
        if (!await db.EmailSpecifications.AnyAsync(e => e.NotificationType == NotificationType.FedExWeeklyChargesSummary))
        {
            var specification = new EmailSpecification
            {
                NotificationType = NotificationType.FedExWeeklyChargesSummary,
                Name = "FedEx Weekly Charges Summary",
                Subject = "FedEx Weekly Charges Summary Report",
                HtmlBody = "<p></p>",
                TextBody = "FedEx Weekly Charges Summary Report",
                FromAddress = "reports@namehere.com",
                FromName = "FedEx Reporting System",
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System",
                LastModifiedOn = DateTime.UtcNow,
                LastModifiedBy = "System"
            };

            await db.EmailSpecifications.AddAsync(specification);
            await db.SaveChangesAsync();

            var recipientGroup = await CreateRecipientGroupAsync(specification.Id, "Logistics Team", "Logistics department recipients");
            
            await AddRecipientsToGroupAsync(recipientGroup.Id, new[]
            {
                ("manager@namehere.com", "Logistics Manager", RecipientType.Cc)
            });
        }
    }

    private async Task SeedFedExWeeklyDetailChargesSummaryAsync()
    {
        if (!await db.EmailSpecifications.AnyAsync(e => e.NotificationType == NotificationType.FedExWeeklyDetailChargesSummary))
        {
            var specification = new EmailSpecification
            {
                NotificationType = NotificationType.FedExWeeklyDetailChargesSummary,
                Name = "FedEx Weekly Detail Charges Summary",
                Subject = "FedEx Weekly Detail Charges Summary Report",
                HtmlBody = "<p></p>",
                TextBody = "FedEx Weekly Detail Charges Summary Report",
                FromAddress = "reports@namehere.com",
                FromName = "FedEx Reporting System",
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System",
                LastModifiedOn = DateTime.UtcNow,
                LastModifiedBy = "System"
            };

            await db.EmailSpecifications.AddAsync(specification);
            await db.SaveChangesAsync();

            var recipientGroup = await CreateRecipientGroupAsync(specification.Id, "Logistics Team", "Logistics department recipients");
            
            await AddRecipientsToGroupAsync(recipientGroup.Id, new[]
            {
                ("logistics@namehere.com", "Logistics Team", RecipientType.To),
            });
        }
    }

    private async Task SeedFedExRemittanceSummaryAsync()
    {
        if (!await db.EmailSpecifications.AnyAsync(e => e.NotificationType == NotificationType.FedExRemittanceSummary))
        {
            var specification = new EmailSpecification
            {
                NotificationType = NotificationType.FedExRemittanceSummary,
                Name = "FedEx Remittance Summary",
                Subject = "FedEx Remittance Summary Report",
                HtmlBody = "<p></p>",
                TextBody = "FedEx Remittance Summary Report",
                FromAddress = "reports@namehere.com",
                FromName = "FedEx Reporting System",
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System",
                LastModifiedOn = DateTime.UtcNow,
                LastModifiedBy = "System"
            };

            await db.EmailSpecifications.AddAsync(specification);
            await db.SaveChangesAsync();

            var recipientGroup = await CreateRecipientGroupAsync(specification.Id, "Finance Team", "Finance department recipients");
            
            await AddRecipientsToGroupAsync(recipientGroup.Id, new[]
            {
                ("finance@namehere.com", "Finance Team", RecipientType.To),
            });
        }
    }

    private async Task SeedFedExRemittanceDetailsAsync()
    {
        if (!await db.EmailSpecifications.AnyAsync(e => e.NotificationType == NotificationType.FedExRemittanceDetails))
        {
            var specification = new EmailSpecification
            {
                NotificationType = NotificationType.FedExRemittanceDetails,
                Name = "FedEx Remittance Details",
                Subject = "FedEx Remittance Details Report",
                HtmlBody = "<p></p>",
                TextBody = "FedEx Remittance Details Report",
                FromAddress = "reports@namehere.com",
                FromName = "FedEx Reporting System",
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System",
                LastModifiedOn = DateTime.UtcNow,
                LastModifiedBy = "System"
            };

            await db.EmailSpecifications.AddAsync(specification);
            await db.SaveChangesAsync();

            var recipientGroup = await CreateRecipientGroupAsync(specification.Id, "Finance Team", "Finance department recipients");
            
            await AddRecipientsToGroupAsync(recipientGroup.Id, new[]
            {
                ("finance@namehere.com", "Finance Team", RecipientType.To),
                ("accounting@namehere.com", "Accounting Team", RecipientType.Cc)
            });
        }
    }

    private async Task SeedFedExFileReceiptAsync()
    {
        if (!await db.EmailSpecifications.AnyAsync(e => e.NotificationType == NotificationType.FedExFileReceipt))
        {
            var specification = new EmailSpecification
            {
                NotificationType = NotificationType.FedExFileReceipt,
                Name = "FedEx File Receipt",
                Subject = "FedEx File Received and Processed",
                HtmlBody = "<p></p>",
                TextBody = "FedEx File Received and Processed",
                FromAddress = "reports@namehere.com",
                FromName = "FedEx Reporting System",
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System",
                LastModifiedOn = DateTime.UtcNow,
                LastModifiedBy = "System"
            };

            await db.EmailSpecifications.AddAsync(specification);
            await db.SaveChangesAsync();

            var recipientGroup = await CreateRecipientGroupAsync(specification.Id, "Operations Team", "Operations department recipients");
            
            await AddRecipientsToGroupAsync(recipientGroup.Id, new[]
            {
                ("operations@namehere.com", "Operations Team", RecipientType.To),
            });
        }
    }

    private async Task SeedFedExFileMissingAsync()
    {
        if (!await db.EmailSpecifications.AnyAsync(e => e.NotificationType == NotificationType.FedExFileMissing))
        {
            var specification = new EmailSpecification
            {
                NotificationType = NotificationType.FedExFileMissing,
                Name = "FedEx File Missing",
                Subject = "FedEx File Missing Notification",
                HtmlBody = "<p></p>",
                TextBody = "FedEx File Missing Notification",
                FromAddress = "reports@namehere.com",
                FromName = "FedEx Reporting System",
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System",
                LastModifiedOn = DateTime.UtcNow,
                LastModifiedBy = "System"
            };

            await db.EmailSpecifications.AddAsync(specification);
            await db.SaveChangesAsync();

            var recipientGroup = await CreateRecipientGroupAsync(specification.Id, "Operations Team", "Operations department recipients");
            
            await AddRecipientsToGroupAsync(recipientGroup.Id, new[]
            {
                ("manager@namehere.com", "Operations Manager", RecipientType.Cc)
            });
        }
    }

    private async Task SeedReassignedTrackingNumbersAsync()
    {
        if (!await db.EmailSpecifications.AnyAsync(e => e.NotificationType == NotificationType.DailyReassignedTrackingNumbers))
        {
            var specification = new EmailSpecification
            {
                NotificationType = NotificationType.DailyReassignedTrackingNumbers,
                Name = "Daily Reassigned Tracking Numbers",
                Subject = "Daily Reassigned Tracking Numbers Report",
                HtmlBody = "<p></p>",
                TextBody = "Daily Reassigned Tracking Numbers Report",
                FromAddress = "reports@namehere.com",
                FromName = "FedEx Reporting System",
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System",
                LastModifiedOn = DateTime.UtcNow,
                LastModifiedBy = "System"
            };

            await db.EmailSpecifications.AddAsync(specification);
            await db.SaveChangesAsync();

            var recipientGroup = await CreateRecipientGroupAsync(specification.Id, "Logistics Team", "Logistics department recipients");
            
            await AddRecipientsToGroupAsync(recipientGroup.Id, new[]
            {
                ("operations@namehere.com", "Operations Team", RecipientType.Cc)
            });
        }
    }

    private async Task SeedDelayedInvoicesAsync()
    {
        if (!await db.EmailSpecifications.AnyAsync(e => e.NotificationType == NotificationType.DelayedInvoicesReport))
        {
            var specification = new EmailSpecification
            {
                NotificationType = NotificationType.DelayedInvoicesReport,
                Name = "Delayed Invoices Report",
                Subject = "Delayed Invoices Report",
                HtmlBody = "<p></p>",
                TextBody = "Delayed Invoices Report",
                FromAddress = "reports@namehere.com",
                FromName = "Finance Reporting System",
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System",
                LastModifiedOn = DateTime.UtcNow,
                LastModifiedBy = "System"
            };

            await db.EmailSpecifications.AddAsync(specification);
            await db.SaveChangesAsync();

            var recipientGroup = await CreateRecipientGroupAsync(specification.Id, "Finance Team", "Finance department recipients");
            
            await AddRecipientsToGroupAsync(recipientGroup.Id, new[]
            {
                ("finance@namehere.com", "Finance Team", RecipientType.To),
            });
        }
    }

    private async Task SeedPendingApprovalAsync()
    {
        if (!await db.EmailSpecifications.AnyAsync(e => e.NotificationType == NotificationType.PendingApprovalNotification))
        {
            var specification = new EmailSpecification
            {
                NotificationType = NotificationType.PendingApprovalNotification,
                Name = "Pending Approval Notification",
                Subject = "Items Pending Your Approval",
                HtmlBody = "<p></p>",
                TextBody = "Items Pending Your Approval",
                FromAddress = "reports@namehere.com",
                FromName = "Approval System",
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System",
                LastModifiedOn = DateTime.UtcNow,
                LastModifiedBy = "System"
            };

            await db.EmailSpecifications.AddAsync(specification);
            await db.SaveChangesAsync();

            var recipientGroup = await CreateRecipientGroupAsync(specification.Id, "Approvers", "Approval team recipients");
            
            await AddRecipientsToGroupAsync(recipientGroup.Id, new[]
            {
                ("approvals@namehere.com", "Approvals Team", RecipientType.To)
            });
        }
    }

    private async Task SeedTrackingNumbersByBusinessUnitAsync()
    {
        if (!await db.EmailSpecifications.AnyAsync(e => e.NotificationType == NotificationType.WeeklyTrackingByBusinessUnit))
        {
            var specification = new EmailSpecification
            {
                NotificationType = NotificationType.WeeklyTrackingByBusinessUnit,
                Name = "Weekly Tracking by Business Unit",
                Subject = "Weekly Tracking by Business Unit Report",
                HtmlBody = "<p></p>",
                TextBody = "Weekly Tracking by Business Unit Report",
                FromAddress = "reports@namehere.com",
                FromName = "FedEx Reporting System",
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System",
                LastModifiedOn = DateTime.UtcNow,
                LastModifiedBy = "System"
            };

            await db.EmailSpecifications.AddAsync(specification);
            await db.SaveChangesAsync();

            var recipientGroup = await CreateRecipientGroupAsync(specification.Id, "Business Units", "Business unit managers");
            
            await AddRecipientsToGroupAsync(recipientGroup.Id, new[]
            {
                ("managers@namehere.com", "Business Unit Managers", RecipientType.To),
            });
        }
    }

    private async Task SeedInvalidEmployeeIdAsync()
    {
        if (!await db.EmailSpecifications.AnyAsync(e => e.NotificationType == NotificationType.InvalidEmployeeIdSummary))
        {
            var specification = new EmailSpecification
            {
                NotificationType = NotificationType.InvalidEmployeeIdSummary,
                Name = "Invalid Employee ID Summary",
                Subject = "Invalid Employee ID Summary Report",
                HtmlBody = "<p></p>",
                TextBody = "Invalid Employee ID Summary Report",
                FromAddress = "reports@namehere.com",
                FromName = "HR Reporting System",
                IsActive = true,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = "System",
                LastModifiedOn = DateTime.UtcNow,
                LastModifiedBy = "System"
            };

            await db.EmailSpecifications.AddAsync(specification);
            await db.SaveChangesAsync();

            var recipientGroup = await CreateRecipientGroupAsync(specification.Id, "HR Team", "HR department recipients");
            
            await AddRecipientsToGroupAsync(recipientGroup.Id, new[]
            {
                ("hr@namehere.com", "HR Team", RecipientType.To),
            });
        }
    }

    private async Task<EmailRecipientGroup> CreateRecipientGroupAsync(int emailSpecificationId, string name, string description)
    {
        var group = new EmailRecipientGroup
        {
            Name = name,
            Description = description,
            EmailSpecificationId = emailSpecificationId,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = "System",
            LastModifiedOn = DateTime.UtcNow,
            LastModifiedBy = "System"
        };

        await db.EmailRecipientGroups.AddAsync(group);
        await db.SaveChangesAsync();
        return group;
    }

    //
    private async Task AddRecipientsToGroupAsync(int groupId, IEnumerable<(string EmailAddress, string DisplayName, RecipientType Type)> recipients)
    {
        var emailRecipients = recipients.Select(r => new EmailRecipient
        {
            EmailAddress = r.EmailAddress,
            DisplayName = r.DisplayName,
            Type = r.Type,
            EmailRecipientGroupId = groupId,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = "System",
            LastModifiedOn = DateTime.UtcNow,
            LastModifiedBy = "System"
        }).ToList();

        await db.EmailRecipients.AddRangeAsync(emailRecipients);
        await db.SaveChangesAsync();
    }
} 