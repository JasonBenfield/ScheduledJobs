using XTI_ScheduledJobsWebAppApi.JobDefinitions;

namespace XTI_ScheduledJobsWebAppApi;

internal static class JobDefinitionsGroupExtensions
{
    public static void AddJobDefinitionsGroupServices(this IServiceCollection services)
    {
        services.AddScoped<IndexView>();
        services.AddScoped<GetJobDefinitionsAction>();
        services.AddScoped<GetRecentTriggeredJobsAction>();
    }
}