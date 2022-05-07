using XTI_ScheduledJobsWebAppApi.Home;

namespace XTI_ScheduledJobsWebAppApi;

internal static class HomeGroupExtensions
{
    public static void AddHomeGroupServices(this IServiceCollection services)
    {
        services.AddScoped<IndexAction>();
    }
}