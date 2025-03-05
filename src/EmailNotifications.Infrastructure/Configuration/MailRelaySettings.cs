namespace EmailNotifications.Infrastructure.Configuration;

/// <summary>
/// Configuration settings for the mail relay server
/// </summary>
public class MailRelaySettings
{
    /// <summary>
    /// Gets or sets the mail relay server hostname
    /// </summary>
    public required string Server { get; init; }

    /// <summary>
    /// Gets or sets the mail relay server port (should be 587 for SMTP over TLS)
    /// </summary>
    public int Port { get; init; } = 587;

    /// <summary>
    /// Gets or sets whether to use SSL/TLS for the connection
    /// </summary>
    public bool UseSsl { get; init; } = true;

    /// <summary>
    /// Gets or sets the path to the intermediate certificate file
    /// </summary>
    public string? IntermediateCertificatePath { get; init; }

    /// <summary>
    /// Gets or sets the sender's email address
    /// </summary>
    public required string SenderEmail { get; init; }

    /// <summary>
    /// Gets or sets the sender's display name
    /// </summary>
    public required string SenderName { get; init; }

    /// <summary>
    /// Maximum number of retry attempts when sending fails with a transient error
    /// </summary>
    public int MaxRetryAttempts { get; init; } = 3;

    /// <summary>
    /// Initial delay between retry attempts in milliseconds
    /// </summary>
    public int RetryDelayMilliseconds { get; init; } = 1000;
} 