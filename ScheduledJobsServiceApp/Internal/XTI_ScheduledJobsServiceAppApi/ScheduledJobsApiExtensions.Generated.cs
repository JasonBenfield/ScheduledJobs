// Generated Code
namespace XTI_ScheduledJobsServiceAppApi;
public static partial class ScheduledJobsApiExtensions
{
    public static void AddScheduledJobsAppApiServices(this IServiceCollection services)
    {
        services.AddJobsServices();
        services.AddScoped<AppApiFactory, ScheduledJobsAppApiFactory>();
        services.AddMoreServices();
    }

    static partial void AddMoreServices(this IServiceCollection services);
}