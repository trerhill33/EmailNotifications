namespace EmailNotifications.Domain.Enums;

/// <summary>
/// Represents the status of an email in the sending process
/// </summary>
public enum EmailStatus
{
    /// <summary>
    /// Email is pending to be sent
    /// </summary>
    Pending = 1,

    /// <summary>
    /// Email is currently being processed for sending
    /// </summary>
    Processing = 2,

    /// <summary>
    /// Email was sent successfully
    /// </summary>
    Sent = 3,

    /// <summary>
    /// Email sending failed but will be retried
    /// </summary>
    Failed = 4,

    /// <summary>
    /// Email sending failed permanently after all retry attempts
    /// </summary>
    FailedPermanently = 5
}