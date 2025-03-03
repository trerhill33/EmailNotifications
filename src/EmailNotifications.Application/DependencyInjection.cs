using EmailNotifications.Application.Interfaces;
using EmailNotifications.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EmailNotifications.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<PasswordResetService>();

        return services;
    }
}