namespace XTI_ScheduleJobWebAppApi;

public static class ScheduleJobAppApiExtensions
{
    public static void AddScheduleJobAppApiServices(this IServiceCollection services)
    {
        services.AddHomeGroupServices();
    }
}