// Generated Code
using Microsoft.Extensions.DependencyInjection;

namespace XTI_ScheduledJobsAppClient;
public static class ScheduledJobsAppClientExtensions
{
    public static void AddScheduledJobsAppClient(this IServiceCollection services)
    {
        services.AddScoped<ScheduledJobsAppClient>();
        services.AddScoped<ScheduledJobsAppClientVersion>();
    }
}