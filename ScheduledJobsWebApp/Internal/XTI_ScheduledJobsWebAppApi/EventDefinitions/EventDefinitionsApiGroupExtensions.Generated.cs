using XTI_ScheduledJobsWebAppApiActions.EventDefinitions;

// Generated Code
namespace XTI_ScheduledJobsWebAppApi;
internal static partial class EventDefinitionsApiGroupExtensions
{
    internal static void AddEventDefinitionsServices(this IServiceCollection services)
    {
        services.AddScoped<GetEventDefinitionsAction>();
        services.AddScoped<GetRecentNotificationsAction>();
        services.AddScoped<IndexPage>();
    }
}