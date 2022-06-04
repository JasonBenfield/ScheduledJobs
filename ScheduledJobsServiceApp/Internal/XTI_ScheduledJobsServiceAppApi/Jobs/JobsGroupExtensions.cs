using XTI_ScheduledJobsServiceAppApi.Jobs;

namespace XTI_ScheduledJobsServiceAppApi;

internal static class JobsGroupExtensions
{
    public static void AddHomeGroupServices(this IServiceCollection services)
    {
        services.AddScoped<PurgeJobsAndEventsAction>();
    }
}