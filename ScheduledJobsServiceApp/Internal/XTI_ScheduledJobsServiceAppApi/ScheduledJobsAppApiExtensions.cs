namespace XTI_ScheduledJobsServiceAppApi;

public static class ScheduledJobsAppApiExtensions
{
    public static void AddScheduledJobsAppApiServices(this IServiceCollection services)
    {
        services.AddJobsGroupServices();
    }
}