namespace EmailNotifications.Domain.Enums;

/// <summary>
/// Represents the type of email recipient
/// </summary>
public enum RecipientType
{
    /// <summary>
    /// Primary recipient in the To field
    /// </summary>
    To = 1,

    /// <summary>
    /// Carbon copy recipient in the CC field
    /// </summary>
    CC = 2,

    /// <summary>
    /// Blind carbon copy recipient in the BCC field
    /// </summary>
    BCC = 3
}