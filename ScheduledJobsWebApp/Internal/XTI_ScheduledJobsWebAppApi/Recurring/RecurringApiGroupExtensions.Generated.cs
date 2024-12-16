using XTI_ScheduledJobsWebAppApiActions.Recurring;

// Generated Code
namespace XTI_ScheduledJobsWebAppApi;
internal static partial class RecurringApiGroupExtensions
{
    internal static void AddRecurringServices(this IServiceCollection services)
    {
        services.AddScoped<AddJobScheduleNotificationsAction>();
        services.AddScoped<PurgeJobsAndEventsAction>();
        services.AddScoped<TimeoutTasksAction>();
    }
}