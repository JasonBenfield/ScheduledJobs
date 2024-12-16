using XTI_ScheduledJobsWebAppApiActions.Jobs;

// Generated Code
namespace XTI_ScheduledJobsWebAppApi;
internal static partial class JobsApiGroupExtensions
{
    internal static void AddJobsServices(this IServiceCollection services)
    {
        services.AddScoped<AddOrUpdateJobSchedulesAction>();
        services.AddScoped<AddOrUpdateRegisteredJobsAction>();
        services.AddScoped<DeleteJobsWithNoTasksAction>();
        services.AddScoped<JobCancelledAction>();
        services.AddScoped<LogMessageAction>();
        services.AddScoped<RetryJobsAction>();
        services.AddScoped<StartJobAction>();
        services.AddScoped<StartTaskAction>();
        services.AddScoped<TaskCompletedAction>();
        services.AddScoped<TaskFailedAction>();
        services.AddScoped<TriggerJobsAction>();
    }
}