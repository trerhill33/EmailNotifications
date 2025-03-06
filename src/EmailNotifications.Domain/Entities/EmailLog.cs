using EmailNotifications.Domain;
using EmailNotifications.Domain.Enums;

namespace EmailNotifications.Domain.Entities;

/// <summary>
/// Represents a log entry for an email that was sent or attempted to be sent
/// </summary>
public sealed class EmailLog : AuditableEntity
{
    /// <summary>
    /// The notification type associated with this email log
    /// </summary>
    public NotificationType NotificationType { get; set; }

    /// <summary>
    /// The ID of the email specification used for this email
    /// </summary>
    public int EmailSpecificationId { get; set; }

    /// <summary>
    /// The email specification used for this email
    /// </summary>
    public EmailSpecification? EmailSpecification { get; set; }

    /// <summary>
    /// The subject of the email
    /// </summary>
    public required string Subject { get; set; }

    /// <summary>
    /// The sender's email address
    /// </summary>
    public required string FromAddress { get; set; }

    /// <summary>
    /// The sender's display name
    /// </summary>
    public string? FromName { get; set; }

    /// <summary>
    /// Comma-separated list of recipient email addresses
    /// </summary>
    public required string ToAddresses { get; set; }

    /// <summary>
    /// Comma-separated list of CC recipient email addresses
    /// </summary>
    public string? CcAddresses { get; set; }

    /// <summary>
    /// Comma-separated list of BCC recipient email addresses
    /// </summary>
    public string? BccAddresses { get; set; }

    /// <summary>
    /// The status of the email sending process
    /// </summary>
    public EmailStatus Status { get; set; } = EmailStatus.Pending;

    /// <summary>
    /// The date and time when the email was sent
    /// </summary>
    public DateTime? SentAt { get; set; }

    /// <summary>
    /// The number of attempts made to send the email
    /// </summary>
    public int AttemptCount { get; set; } = 0;

    /// <summary>
    /// The date and time when the next attempt should be made
    /// </summary>
    public DateTime? NextAttemptAt { get; set; }

    /// <summary>
    /// Error message if the email sending failed
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// The serialized data model used for rendering the email template
    /// </summary>
    public string? SerializedModel { get; set; }
}