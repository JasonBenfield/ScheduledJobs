// Generated Code
using Microsoft.Extensions.DependencyInjection;

namespace XTI_ScheduledJobsAppClient;
public static class ScheduledJobsAppClientExtensions
{
    public static void AddScheduledJobsAppClient(this IServiceCollection services)
    {
        services.TryAddScoped<IAppClientSessionKey, EmptyAppClientSessionKey>();
        services.TryAddScoped<IAppClientRequestKey, EmptyAppClientRequestKey>();
        services.AddScoped<ScheduledJobsAppClientFactory>();
        services.AddScoped(sp => sp.GetRequiredService<ScheduledJobsAppClientFactory>().Create());
        services.AddScoped<ScheduledJobsAppClientVersion>();
    }
}