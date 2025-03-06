using EmailNotifications.Application.Reports;
using EmailNotifications.Application.Reports.Services;
using EmailNotifications.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EmailNotifications.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register report services
        services.AddScoped<IDailyReportService, DailyReportService>();
        services.AddScoped<IWeeklyReportService, WeeklyReportService>();
        
        return services;
    }
}