namespace EmailNotifications.Application.Models;

/// <summary>
/// Model for the welcome email template
/// </summary>
public sealed record WelcomeTemplateModel(string FirstName) : ITemplateModel; 