namespace XTI_ScheduledJobsWebAppApi.Jobs;

public sealed class JobsGroup : AppApiGroupWrapper
{
    public JobsGroup(AppApiGroup source, IServiceProvider sp)
        : base(source)
    {
        AddOrUpdateJobSchedules = source.AddAction
        (
            nameof(AddOrUpdateJobSchedules), 
            () => sp.GetRequiredService<AddOrUpdateJobSchedulesAction>()
        );
        AddOrUpdateRegisteredJobs = source.AddAction
        (
            nameof(AddOrUpdateRegisteredJobs), 
            () => sp.GetRequiredService<AddOrUpdateRegisteredJobsAction>()
        );
        TriggerJobs = source.AddAction
        (
            nameof(TriggerJobs), 
            () => sp.GetRequiredService<TriggerJobsAction>()
        );
        DeleteJobsWithNoTasks = source.AddAction
        (
            nameof(DeleteJobsWithNoTasks), 
            () => sp.GetRequiredService<DeleteJobsWithNoTasksAction>()
        );
        RetryJobs = source.AddAction
        (
            nameof(RetryJobs), 
            () => sp.GetRequiredService<RetryJobsAction>()
        );
        StartJob = source.AddAction
        (
            nameof(StartJob), 
            () => sp.GetRequiredService<StartJobAction>()
        );
        StartTask = source.AddAction
        (
            nameof(StartTask), 
            () => sp.GetRequiredService<StartTaskAction>()
        );
        JobCancelled = source.AddAction
        (
            nameof(JobCancelled), 
            () => sp.GetRequiredService<JobCancelledAction>()
        );
        TaskCompleted = source.AddAction
        (
            nameof(TaskCompleted), 
            () => sp.GetRequiredService<TaskCompletedAction>()
        );
        TaskFailed = source.AddAction
        (
            nameof(TaskFailed), 
            () => sp.GetRequiredService<TaskFailedAction>()
        );
        LogMessage = source.AddAction
        (
            nameof(LogMessage), 
            () => sp.GetRequiredService<LogMessageAction>()
        );
    }

    public AppApiAction<AddOrUpdateJobSchedulesRequest, EmptyActionResult> AddOrUpdateJobSchedules { get; }
    public AppApiAction<RegisteredJob[], EmptyActionResult> AddOrUpdateRegisteredJobs { get; }
    public AppApiAction<TriggerJobsRequest, PendingJobModel[]> TriggerJobs { get; }
    public AppApiAction<DeleteJobsWithNoTasksRequest, EmptyActionResult> DeleteJobsWithNoTasks { get; }
    public AppApiAction<RetryJobsRequest, TriggeredJobWithTasksModel[]> RetryJobs { get; }
    public AppApiAction<StartJobRequest, TriggeredJobWithTasksModel> StartJob { get; }
    public AppApiAction<StartTaskRequest, EmptyActionResult> StartTask { get; }
    public AppApiAction<JobCancelledRequest, EmptyActionResult> JobCancelled { get; }
    public AppApiAction<TaskCompletedRequest, TriggeredJobWithTasksModel> TaskCompleted { get; }
    public AppApiAction<TaskFailedRequest, TriggeredJobWithTasksModel> TaskFailed { get; }
    public AppApiAction<LogMessageRequest, EmptyActionResult> LogMessage { get; }
}