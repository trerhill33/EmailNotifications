namespace EmailNotifications.Infrastructure.Configuration;

/// <summary>
/// Configuration settings for the mail relay server
/// </summary>
public class MailRelaySettings
    {
        /// <summary>
        /// The SMTP server hostname (e.g., "mailrelay.dev.com").
        /// </summary>
        public required string Server { get; init; }

        /// <summary>
        /// The SMTP server port (e.g., 587).
        /// </summary>
        public required int Port { get; init; }

        /// <summary>
        /// Indicates whether SSL/TLS should be used for the SMTP connection.
        /// </summary>
        public required bool UseSsl { get; init; }

        /// <summary>
        /// Indicates whether custom server certificate validation is enabled.
        /// </summary>
        public bool UseCustomServerCertificateValidation { get; set; }

        /// <summary>
        /// The secret ID in AWS Secrets Manager for the intermediate certificate (e.g., "dev--mail-cert-test").
        /// </summary>
        public string? ServerIntermediateCertificateSecret { get; set; }

        /// <summary>
        /// The timeout of smtp server connection
        /// </summary>
        public required int Timeout { get; set; }
        
        /// <summary>
        /// The maximum number of retry attempts for sending an email (e.g., 3).
        /// </summary>
        public required int MaxRetryAttempts { get; init; }

        /// <summary>
        /// The delay in milliseconds between retry attempts (e.g., 1000).
        /// </summary>
        public required int RetryDelayMilliseconds { get; init; }
    }