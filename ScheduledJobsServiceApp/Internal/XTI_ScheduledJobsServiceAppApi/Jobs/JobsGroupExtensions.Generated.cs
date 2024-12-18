using XTI_ScheduledJobsServiceAppApiActions.Jobs;

// Generated Code
namespace XTI_ScheduledJobsServiceAppApi;
internal static partial class JobsGroupExtensions
{
    internal static void AddJobsServices(this IServiceCollection services)
    {
        services.AddScoped<AddJobScheduleNotificationsAction>();
        services.AddScoped<PurgeJobsAndEventsAction>();
        services.AddScoped<TimeoutJobsAction>();
    }
}