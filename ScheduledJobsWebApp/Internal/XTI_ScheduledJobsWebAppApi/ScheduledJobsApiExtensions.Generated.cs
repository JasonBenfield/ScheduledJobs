// Generated Code
namespace XTI_ScheduledJobsWebAppApi;
public static partial class ScheduledJobsApiExtensions
{
    public static void AddScheduledJobsAppApiServices(this IServiceCollection services)
    {
        services.AddEventDefinitionsServices();
        services.AddEventInquiryServices();
        services.AddEventsServices();
        services.AddHomeServices();
        services.AddJobDefinitionsServices();
        services.AddJobInquiryServices();
        services.AddJobsServices();
        services.AddRecurringServices();
        services.AddTasksServices();
        services.AddScoped<AppApiFactory, ScheduledJobsAppApiFactory>();
        services.AddMoreServices();
    }

    static partial void AddMoreServices(this IServiceCollection services);
}