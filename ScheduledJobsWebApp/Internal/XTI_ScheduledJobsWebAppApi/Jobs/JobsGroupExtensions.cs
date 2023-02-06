using XTI_ScheduledJobsWebAppApi.Jobs;

namespace XTI_ScheduledJobsWebAppApi;

internal static class JobsGroupExtensions
{
    public static void AddJobsGroupServices(this IServiceCollection services)
    {
        services.AddScoped<AddOrUpdateRegisteredJobsAction>();
        services.AddScoped<AddOrUpdateJobSchedulesAction>();
        services.AddScoped<TriggerJobsAction>();
        services.AddScoped<DeleteJobsWithNoTasksAction>();
        services.AddScoped<RetryJobsAction>();
        services.AddScoped<StartJobAction>();
        services.AddScoped<StartTaskAction>();
        services.AddScoped<JobCancelledAction>();
        services.AddScoped<TaskCompletedAction>();
        services.AddScoped<TaskFailedAction>();
        services.AddScoped<LogMessageAction>();
    }
}