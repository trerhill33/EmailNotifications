namespace EmailNotifications.Application.Models;

public sealed record CreateUserTemplateModel(
    string FirstName,
    string LastName,
    string FormattedDate) : ITemplateModel;

public sealed record ResetPasswordTemplateModel(
    string FirstName,
    string OneTimePassword,
    string ExpiryTimeFormatted) : ITemplateModel;

public sealed record PasswordChangedTemplateModel(
    string FirstName,
    DateTime ChangeDate) : ITemplateModel;