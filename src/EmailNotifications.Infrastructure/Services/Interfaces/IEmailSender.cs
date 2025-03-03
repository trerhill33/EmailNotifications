using System.Threading.Tasks;
using EmailNotifications.Infrastructure.Models;

namespace EmailNotifications.Infrastructure.Services.Interfaces;

public interface IEmailSender
{
    Task<(bool Success, string ErrorMessage, int RetryCount)> SendEmailAsync(EmailMessage email);
} 