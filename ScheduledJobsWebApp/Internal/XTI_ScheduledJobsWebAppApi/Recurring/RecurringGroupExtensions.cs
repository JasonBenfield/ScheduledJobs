using XTI_ScheduledJobsWebAppApi.Recurring;

namespace XTI_ScheduledJobsWebAppApi;

internal static class RecurringGroupExtensions
{
    public static void AddRecurringGroupServices(this IServiceCollection services)
    {
        services.AddScoped<AddJobScheduleNotificationsAction>();
        services.AddScoped<TimeoutTasksAction>();
        services.AddScoped<PurgeJobsAndEventsAction>();
    }
}