using XTI_ScheduledJobsWebAppApi.Events;

namespace XTI_ScheduledJobsWebAppApi;

internal static class EventsGroupExtensions
{
    public static void AddEventsGroupServices(this IServiceCollection services)
    {
        services.AddScoped<AddJobScheduleNotificationsAction>();
        services.AddScoped<AddOrUpdateRegisteredEventsAction>();
        services.AddScoped<AddNotificationsAction>();
        services.AddScoped<TriggeredJobsAction>();
    }
}