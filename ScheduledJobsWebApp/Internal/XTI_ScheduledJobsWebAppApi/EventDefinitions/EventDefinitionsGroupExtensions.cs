using XTI_ScheduledJobsWebAppApi.EventDefinitions;

namespace XTI_ScheduledJobsWebAppApi;

internal static class EventDefinitionsGroupExtensions
{
    public static void AddEventDefinitionsGroupServices(this IServiceCollection services)
    {
        services.AddScoped<IndexView>();
        services.AddScoped<GetEventDefinitionsAction>();
        services.AddScoped<GetRecentNotificationsAction>();
    }
}