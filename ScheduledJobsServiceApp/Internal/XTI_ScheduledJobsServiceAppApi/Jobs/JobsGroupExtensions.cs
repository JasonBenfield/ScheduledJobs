using XTI_ScheduledJobsServiceAppApi.Jobs;

namespace XTI_ScheduledJobsServiceAppApi;

internal static class JobsGroupExtensions
{
    public static void AddJobsGroupServices(this IServiceCollection services)
    {
        services.AddScoped<PurgeJobsAndEventsAction>();
        services.AddScoped<TimeoutJobsAction>();
    }
}