using XTI_ScheduleJobWebAppApi.Home;

namespace XTI_ScheduleJobWebAppApi;

internal static class HomeGroupExtensions
{
    public static void AddHomeGroupServices(this IServiceCollection services)
    {
        services.AddScoped<IndexAction>();
    }
}