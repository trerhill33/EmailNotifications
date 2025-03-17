using System;

namespace EmailNotifications.Domain.Enums;

public enum NotificationType
{
    NewUser = 1,
    PasswordReset = 2,
    WeeklySummary = 3,
    PendingApprovals = 4,
} 