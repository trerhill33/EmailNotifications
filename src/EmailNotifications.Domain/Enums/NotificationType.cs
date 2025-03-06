using System;

namespace EmailNotifications.Domain.Enums;

public enum NotificationType
{
    /// <summary>
    /// Notification sent when a new user is created
    /// </summary>
    NewUser = 1,

    /// <summary>
    /// Notification sent when a user requests a password reset
    /// </summary>
    PasswordReset = 2,

    /// <summary>
    /// Welcome email sent to new users
    /// </summary>
    Welcome = 3,

    /// <summary>
    /// FedEx weekly charges summary report
    /// </summary>
    FedExWeeklyChargesSummary = 4,

    /// <summary>
    /// FedEx weekly detail charges summary report
    /// </summary>
    FedExWeeklyDetailChargesSummary = 5,

    /// <summary>
    /// FedEx remittance summary report
    /// </summary>
    FedExRemittanceSummary = 6,

    /// <summary>
    /// FedEx remittance details report
    /// </summary>
    FedExRemittanceDetails = 7,

    /// <summary>
    /// FedEx file receipt notification
    /// </summary>
    FedExFileReceipt = 8,

    /// <summary>
    /// FedEx file missing notification
    /// </summary>
    FedExFileMissing = 9,

    /// <summary>
    /// Daily reassigned tracking numbers report
    /// </summary>
    DailyReassignedTrackingNumbers = 10,

    /// <summary>
    /// Delayed invoices report
    /// </summary>
    DelayedInvoicesReport = 11,

    /// <summary>
    /// Pending approval notification
    /// </summary>
    PendingApprovalNotification = 12,

    /// <summary>
    /// Weekly tracking numbers by business unit report
    /// </summary>
    WeeklyTrackingByBusinessUnit = 13,

    /// <summary>
    /// Invalid employee ID summary report
    /// </summary>
    InvalidEmployeeIdSummary = 14
} 