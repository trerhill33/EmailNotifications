using EmailNotifications.Application.Reports.Reports;
using EmailNotifications.Application.Reports.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EmailNotifications.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register individual report implementations
        services.AddScoped<IPendingApprovalReport, PendingApprovalReport>();
        
        // Register aggregator services
        services.AddScoped<IWeeklyReportService, WeeklyReportService>();
        
        return services;
    }
}