using XTI_ScheduledJobsWebAppApiActions.JobDefinitions;

// Generated Code
namespace XTI_ScheduledJobsWebAppApi;
internal static partial class JobDefinitionsGroupExtensions
{
    internal static void AddJobDefinitionsServices(this IServiceCollection services)
    {
        services.AddScoped<GetJobDefinitionsAction>();
        services.AddScoped<GetRecentTriggeredJobsAction>();
        services.AddScoped<IndexPage>();
    }
}