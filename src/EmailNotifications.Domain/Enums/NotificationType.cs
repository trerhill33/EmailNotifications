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
    /// FedEx monthly delivery performance report
    /// </summary>
    FedExMonthlyDeliveryPerformance = 5,

    /// <summary>
    /// FedEx quarterly cost analysis report
    /// </summary>
    FedExQuarterlyCostAnalysis = 6
} 