using XTI_ScheduledJobsWebAppApiActions.Tasks;

// Generated Code
namespace XTI_ScheduledJobsWebAppApi;
internal static partial class TasksGroupExtensions
{
    internal static void AddTasksServices(this IServiceCollection services)
    {
        services.AddScoped<CancelTaskAction>();
        services.AddScoped<EditTaskDataAction>();
        services.AddScoped<RetryTaskAction>();
        services.AddScoped<SkipTaskAction>();
        services.AddScoped<TimeoutTaskAction>();
    }
}