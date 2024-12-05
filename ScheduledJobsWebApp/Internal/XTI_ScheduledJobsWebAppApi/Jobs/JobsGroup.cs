namespace XTI_ScheduledJobsWebAppApi.Jobs;

public sealed class JobsGroup : AppApiGroupWrapper
{
    public JobsGroup(AppApiGroup source, IServiceProvider sp)
        : base(source)
    {
        AddOrUpdateJobSchedules = source.AddAction<AddOrUpdateJobSchedulesRequest, EmptyActionResult>()
            .Named(nameof(AddOrUpdateJobSchedules))
            .WithExecution<AddOrUpdateJobSchedulesAction>()
            .ThrottleRequestLogging().ForOneHour()
            .ThrottleExceptionLogging().For(5).Minutes()
            .Build();
        AddOrUpdateRegisteredJobs = source.AddAction<RegisteredJob[], EmptyActionResult>()
            .Named(nameof(AddOrUpdateRegisteredJobs))
            .WithExecution<AddOrUpdateRegisteredJobsAction>()
            .Build();
        TriggerJobs = source.AddAction<TriggerJobsRequest, PendingJobModel[]>()
            .Named(nameof(TriggerJobs))
            .WithExecution<TriggerJobsAction>()
            .ThrottleRequestLogging().ForOneHour()
            .ThrottleExceptionLogging().For(5).Minutes()
            .Build();
        DeleteJobsWithNoTasks = source.AddAction<DeleteJobsWithNoTasksRequest, EmptyActionResult>()
            .Named(nameof(DeleteJobsWithNoTasks))
            .WithExecution<DeleteJobsWithNoTasksAction>()
            .ThrottleRequestLogging().ForOneHour()
            .ThrottleExceptionLogging().For(5).Minutes()
            .Build();
        RetryJobs = source.AddAction<RetryJobsRequest, TriggeredJobWithTasksModel[]>()
            .Named(nameof(RetryJobs))
            .WithExecution<RetryJobsAction>()
            .ThrottleRequestLogging().ForOneHour()
            .ThrottleExceptionLogging().For(5).Minutes()
            .Build();
        StartJob = source.AddAction<StartJobRequest, TriggeredJobWithTasksModel>()
            .Named(nameof(StartJob))
            .WithExecution<StartJobAction>()
            .ThrottleRequestLogging().ForOneHour()
            .Build();
        StartTask = source.AddAction<StartTaskRequest, EmptyActionResult>()
            .Named(nameof(StartTask))
            .WithExecution<StartTaskAction>()
            .ThrottleRequestLogging().ForOneHour()
            .Build();
        JobCancelled = source.AddAction<JobCancelledRequest, EmptyActionResult>()
            .Named(nameof(JobCancelled))
            .WithExecution<JobCancelledAction>()
            .Build();
        TaskCompleted = source.AddAction<TaskCompletedRequest, TriggeredJobWithTasksModel>()
            .Named(nameof(TaskCompleted))
            .WithExecution<TaskCompletedAction>()
            .ThrottleRequestLogging().ForOneHour()
            .Build();
        TaskFailed = source.AddAction<TaskFailedRequest, TriggeredJobWithTasksModel>()
            .Named(nameof(TaskFailed))
            .WithExecution<TaskFailedAction>()
            .ThrottleRequestLogging().ForOneHour()
            .Build();
        LogMessage = source.AddAction<LogMessageRequest, EmptyActionResult>()
            .Named(nameof(LogMessage))
            .WithExecution<LogMessageAction>()
            .Build();
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