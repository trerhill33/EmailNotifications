using System;

namespace EmailNotifications.Infrastructure.Exceptions;

/// <summary>
/// Base exception for all email-related errors
/// </summary>
public abstract class EmailException : Exception
{
    protected EmailException(string message) : base(message)
    {
    }

    protected EmailException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

/// <summary>
/// Exception thrown when there is an error with the email configuration
/// </summary>
public class EmailConfigurationException : EmailException
{
    public EmailConfigurationException(string message) : base(message)
    {
    }

    public EmailConfigurationException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

/// <summary>
/// Exception thrown when there is an error with the SMTP server connection
/// </summary>
public class SmtpConnectionException : EmailException
{
    public SmtpConnectionException(string message) : base(message)
    {
    }

    public SmtpConnectionException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

/// <summary>
/// Exception thrown when there is an error sending an email
/// </summary>
public class EmailSendException : EmailException
{
    public EmailSendException(string message) : base(message)
    {
    }

    public EmailSendException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

/// <summary>
/// Exception thrown when there is an error with email attachments
/// </summary>
public class EmailAttachmentException : EmailException
{
    public EmailAttachmentException(string message) : base(message)
    {
    }

    public EmailAttachmentException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

/// <summary>
/// Exception thrown when there is an error with email templates
/// </summary>
public class EmailTemplateException : EmailException
{
    public EmailTemplateException(string message) : base(message)
    {
    }

    public EmailTemplateException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

/// <summary>
/// Exception thrown when there is an error with email recipients
/// </summary>
public class EmailRecipientException : EmailException
{
    public EmailRecipientException(string message) : base(message)
    {
    }

    public EmailRecipientException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

/// <summary>
/// Exception thrown when there is an error with email content
/// </summary>
public class EmailContentException : EmailException
{
    public EmailContentException(string message) : base(message)
    {
    }

    public EmailContentException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

/// <summary>
/// Exception thrown when there is an error with email validation
/// </summary>
public class EmailValidationException : EmailException
{
    public EmailValidationException(string message) : base(message)
    {
    }

    public EmailValidationException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

/// <summary>
/// Exception thrown when there is an error with email rate limiting
/// </summary>
public class EmailRateLimitException : EmailException
{
    public EmailRateLimitException(string message) : base(message)
    {
    }

    public EmailRateLimitException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

/// <summary>
/// Exception thrown when there is an error with email authentication
/// </summary>
public class EmailAuthenticationException : EmailException
{
    public EmailAuthenticationException(string message) : base(message)
    {
    }

    public EmailAuthenticationException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
}

/// <summary>
/// Exception thrown when there is an error with email encryption
/// </summary>
public class EmailEncryptionException : EmailException
{
    public EmailEncryptionException(string message) : base(message)
    {
    }

    public EmailEncryptionException(string message, Exception innerException) 
        : base(message, innerException)
    {
    }
} 