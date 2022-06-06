namespace XTI_ScheduledJobsWebAppApi;

public static class ScheduledJobsAppApiExtensions
{
    public static void AddScheduledJobsAppApiServices(this IServiceCollection services)
    {
        services.AddHomeGroupServices();
        services.AddRecurringGroupServices();
        services.AddEventInquiryGroupServices();
        services.AddEventsGroupServices();
        services.AddJobInquiryGroupServices();
        services.AddJobsGroupServices();
        services.AddTasksGroupServices();
    }
}