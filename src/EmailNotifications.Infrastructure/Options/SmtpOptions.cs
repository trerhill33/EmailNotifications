using System.ComponentModel.DataAnnotations;

namespace EmailNotifications.Infrastructure.Options;

/// <summary>
/// Configuration options for SMTP settings
/// </summary>
public class SmtpOptions
{
    /// <summary>
    /// The SMTP server host
    /// </summary>
    [Required]
    public string Host { get; set; } = string.Empty;

    /// <summary>
    /// The SMTP server port
    /// </summary>
    [Required]
    public int Port { get; set; }

    /// <summary>
    /// Whether to enable SSL/TLS
    /// </summary>
    public bool EnableSsl { get; set; }

    /// <summary>
    /// The SMTP username
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// The SMTP password
    /// </summary>
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// Maximum number of retry attempts
    /// </summary>
    public int MaxRetryAttempts { get; set; } = 3;

    /// <summary>
    /// Delay between retry attempts in milliseconds
    /// </summary>
    public int RetryDelayMilliseconds { get; set; } = 1000;
} 