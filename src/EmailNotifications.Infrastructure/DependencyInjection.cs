using EmailNotifications.Application.Interfaces;
using EmailNotifications.Application.Services;
using EmailNotifications.Infrastructure.Configuration;
using EmailNotifications.Infrastructure.Interfaces;
using EmailNotifications.Infrastructure.Persistence;
using EmailNotifications.Infrastructure.Persistence.Repositories;
using EmailNotifications.Infrastructure.Persistence.Seeders;
using EmailNotifications.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

        // Configure mail relay settings
        services.Configure<MailRelaySettings>(configuration.GetSection("MailRelay"));

        // Add infrastructure services
        services.AddScoped<IEmailSender, SmtpEmailSender>();
        services.AddScoped<ITemplateRenderer, ScribanEmailTemplateRenderer>();

        // Add application services
        services.AddScoped<INotificationService, NotificationService>();

        // Register the database seeder
        services.AddScoped<DatabaseSeeder>();

        return services;
    }

    public static async Task SeedDatabaseAsync(this IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
        await seeder.SeedAsync();
    }
}