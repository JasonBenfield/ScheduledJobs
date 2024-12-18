using XTI_ScheduledJobsWebAppApiActions.Events;

// Generated Code
namespace XTI_ScheduledJobsWebAppApi;
internal static partial class EventsGroupExtensions
{
    internal static void AddEventsServices(this IServiceCollection services)
    {
        services.AddScoped<AddNotificationsAction>();
        services.AddScoped<AddOrUpdateRegisteredEventsAction>();
        services.AddScoped<TriggeredJobsAction>();
    }
}