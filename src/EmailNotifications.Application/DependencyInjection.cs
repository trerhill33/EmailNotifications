using EmailNotifications.Application.Reports;
using EmailNotifications.Application.Reports.Interfaces;
using EmailNotifications.Application.Reports.Reports;
using EmailNotifications.Application.Reports.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EmailNotifications.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        // Register individual report implementations
        services.AddScoped<IFedExFileMissingReport, FedExFileMissingReport>();
        services.AddScoped<IFedExRemittanceSummaryReport, FedExRemittanceSummaryReport>();
        services.AddScoped<IFedExRemittanceDetailsReport, FedExRemittanceDetailsReport>();
        services.AddScoped<IFedExWeeklyChargesSummaryReport, FedExWeeklyChargesSummaryReport>();
        services.AddScoped<IFedExWeeklyDetailChargesSummaryReport, FedExWeeklyDetailChargesSummaryReport>();
        services.AddScoped<IFedExFileReceiptReport, FedExFileReceiptReport>();
        services.AddScoped<ITrackingNumbersByBusinessUnitReport, TrackingNumbersByBusinessUnitReport>();
        services.AddScoped<IInvalidEmployeeIdReport, InvalidEmployeeIdReport>();
        services.AddScoped<IReassignedTrackingNumbersReport, ReassignedTrackingNumbersReport>();
        services.AddScoped<IDelayedInvoicesReport, DelayedInvoicesReport>();
        services.AddScoped<IPendingApprovalNotificationsReport, PendingApprovalNotificationsReport>();
        
        // Register aggregator services
        services.AddScoped<IDailyReportService, DailyReportService>();
        services.AddScoped<IWeeklyReportService, WeeklyReportService>();
        
        return services;
    }
}