using EmailNotifications.Application.Interfaces;
using EmailNotifications.Infrastructure.Interfaces;
using EmailNotifications.Infrastructure.Persistence;
using EmailNotifications.Infrastructure.Repositories;
using EmailNotifications.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Net.Mail;

namespace EmailNotifications.Infrastructure;

/// <summary>
/// Extension methods for configuring dependency injection for the infrastructure layer
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Adds the infrastructure services to the service collection
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="configuration">The configuration</param>
    /// <returns>The service collection</returns>
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Add database context
        services.AddDbContext<NotificationDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(NotificationDbContext).Assembly.FullName)
                      .MigrationsHistoryTable("__EFMigrationsHistory", "notification")
                      .UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery)));

        // Add repositories
        services.AddScoped<IEmailSpecificationRepository, EmailSpecificationRepository>();

        // Add SMTP client
        services.AddSingleton(provider =>
        {
            var smtpConfig = configuration.GetSection("Smtp");

            var smtpClient = new SmtpClient
            {
                Host = smtpConfig["Host"] ?? "localhost",
                Port = int.Parse(smtpConfig["Port"] ?? "25"),
                EnableSsl = bool.Parse(smtpConfig["EnableSsl"] ?? "false"),
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            // Add credentials if provided
            if (!string.IsNullOrEmpty(smtpConfig["Username"]) && !string.IsNullOrEmpty(smtpConfig["Password"]))
            {
                smtpClient.Credentials = new NetworkCredential(
                    smtpConfig["Username"],
                    smtpConfig["Password"]);
            }

            return smtpClient;
        });

        // Add infrastructure services
        services.AddScoped<IEmailSender, SmtpEmailSender>();
        services.AddScoped<ITemplateRenderer, ScribanEmailTemplateRenderer>();

        // Add application services
        services.AddScoped<INotificationService, NotificationService>();

        return services;
    }

    // Implementation of IServiceProviderAccessor for DI registration
    private class ServiceProviderAccessorImpl : IServiceProviderAccessor
    {
        public ServiceProviderAccessorImpl(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public IServiceProvider ServiceProvider { get; }
    }
}