using EmailNotifications.Application.Common.Notifications.Interfaces;

namespace EmailNotifications.Application.Common.Notifications.Models;

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