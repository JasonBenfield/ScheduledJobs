using XTI_ScheduledJobsWebAppApi.Tasks;

namespace XTI_ScheduledJobsWebAppApi;

internal static class TasksGroupExtensions
{
    public static void AddTasksGroupServices(this IServiceCollection services)
    {
        services.AddScoped<CancelTaskAction>();
        services.AddScoped<RetryTaskAction>();
    }
}
