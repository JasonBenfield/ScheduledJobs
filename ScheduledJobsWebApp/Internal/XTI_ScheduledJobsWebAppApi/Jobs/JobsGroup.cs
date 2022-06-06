namespace XTI_ScheduledJobsWebAppApi.Jobs;

public sealed class JobsGroup : AppApiGroupWrapper
{
    public JobsGroup(AppApiGroup source, IServiceProvider sp)
        : base(source)
    {
        var actions = new AppApiActionFactory(source);
        AddOrUpdateRegisteredJobs = source.AddAction
        (
            actions.Action(nameof(AddOrUpdateRegisteredJobs), () => sp.GetRequiredService<AddOrUpdateRegisteredJobsAction>())
        );
        TriggerJobs = source.AddAction
        (
            actions.Action(nameof(TriggerJobs), () => sp.GetRequiredService<TriggerJobsAction>())
        );
        RetryJobs = source.AddAction
        (
            actions.Action(nameof(RetryJobs), () => sp.GetRequiredService<RetryJobsAction>())
        );
        StartJob = source.AddAction
        (
            actions.Action(nameof(StartJob), () => sp.GetRequiredService<StartJobAction>())
        );
        StartTask = source.AddAction
        (
            actions.Action(nameof(StartTask), () => sp.GetRequiredService<StartTaskAction>())
        );
        TaskCompleted = source.AddAction
        (
            actions.Action(nameof(TaskCompleted), () => sp.GetRequiredService<TaskCompletedAction>())
        );
        TaskFailed = source.AddAction
        (
            actions.Action(nameof(TaskFailed), () => sp.GetRequiredService<TaskFailedAction>())
        );
        LogMessage = source.AddAction
        (
            actions.Action(nameof(LogMessage), () => sp.GetRequiredService<LogMessageAction>())
        );
    }

    public AppApiAction<RegisteredJob[], EmptyActionResult> AddOrUpdateRegisteredJobs { get; }
    public AppApiAction<TriggerJobsRequest, PendingJobModel[]> TriggerJobs { get; }
    public AppApiAction<RetryJobsRequest, TriggeredJobWithTasksModel[]> RetryJobs { get; }
    public AppApiAction<StartJobRequest, TriggeredJobWithTasksModel> StartJob { get; }
    public AppApiAction<StartTaskRequest, EmptyActionResult> StartTask { get; }
    public AppApiAction<TaskCompletedRequest, TriggeredJobWithTasksModel> TaskCompleted { get; }
    public AppApiAction<TaskFailedRequest, TriggeredJobWithTasksModel> TaskFailed { get; }
    public AppApiAction<LogMessageRequest, EmptyActionResult> LogMessage { get; }
}