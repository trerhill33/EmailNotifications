namespace EmailNotifications.Application.Models;

/// <summary>
/// Model for user creation notification templates
/// </summary>
public sealed record CreateUserTemplateModel(
    string FirstName,
    string LastName,
    string FormattedDate) : ITemplateModel;

/// <summary>
/// Model for password reset notification templates
/// </summary>
public sealed record ResetPasswordTemplateModel(
    string FirstName,
    string OneTimePassword,
    string ExpiryTimeFormatted) : ITemplateModel;

/// <summary>
/// Model for password changed notification templates
/// </summary>
public sealed record PasswordChangedTemplateModel(
    string FirstName,
    DateTime ChangeDate) : ITemplateModel;