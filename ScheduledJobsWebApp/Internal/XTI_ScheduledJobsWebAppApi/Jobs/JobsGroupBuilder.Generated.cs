using XTI_ScheduledJobsWebAppApiActions.Jobs;

// Generated Code
#nullable enable
namespace XTI_ScheduledJobsWebAppApi.Jobs;
public sealed partial class JobsGroupBuilder
{
    private readonly AppApiGroup source;
    internal JobsGroupBuilder(AppApiGroup source)
    {
        this.source = source;
        AddOrUpdateJobSchedules = source.AddAction<AddOrUpdateJobSchedulesRequest, EmptyActionResult>("AddOrUpdateJobSchedules").WithExecution<AddOrUpdateJobSchedulesAction>();
        AddOrUpdateRegisteredJobs = source.AddAction<RegisteredJob[], EmptyActionResult>("AddOrUpdateRegisteredJobs").WithExecution<AddOrUpdateRegisteredJobsAction>();
        DeleteJobsWithNoTasks = source.AddAction<DeleteJobsWithNoTasksRequest, EmptyActionResult>("DeleteJobsWithNoTasks").WithExecution<DeleteJobsWithNoTasksAction>();
        JobCancelled = source.AddAction<JobCancelledRequest, EmptyActionResult>("JobCancelled").WithExecution<JobCancelledAction>();
        LogMessage = source.AddAction<LogMessageRequest, EmptyActionResult>("LogMessage").WithExecution<LogMessageAction>();
        RetryJobs = source.AddAction<RetryJobsRequest, TriggeredJobWithTasksModel[]>("RetryJobs").WithExecution<RetryJobsAction>();
        StartJob = source.AddAction<StartJobRequest, TriggeredJobWithTasksModel>("StartJob").WithExecution<StartJobAction>();
        StartTask = source.AddAction<StartTaskRequest, EmptyActionResult>("StartTask").WithExecution<StartTaskAction>();
        TaskCompleted = source.AddAction<TaskCompletedRequest, TriggeredJobWithTasksModel>("TaskCompleted").WithExecution<TaskCompletedAction>();
        TaskFailed = source.AddAction<TaskFailedRequest, TriggeredJobWithTasksModel>("TaskFailed").WithExecution<TaskFailedAction>();
        TriggerJobs = source.AddAction<TriggerJobsRequest, PendingJobModel[]>("TriggerJobs").WithExecution<TriggerJobsAction>();
        Configure();
    }

    partial void Configure();
    public AppApiActionBuilder<AddOrUpdateJobSchedulesRequest, EmptyActionResult> AddOrUpdateJobSchedules { get; }
    public AppApiActionBuilder<RegisteredJob[], EmptyActionResult> AddOrUpdateRegisteredJobs { get; }
    public AppApiActionBuilder<DeleteJobsWithNoTasksRequest, EmptyActionResult> DeleteJobsWithNoTasks { get; }
    public AppApiActionBuilder<JobCancelledRequest, EmptyActionResult> JobCancelled { get; }
    public AppApiActionBuilder<LogMessageRequest, EmptyActionResult> LogMessage { get; }
    public AppApiActionBuilder<RetryJobsRequest, TriggeredJobWithTasksModel[]> RetryJobs { get; }
    public AppApiActionBuilder<StartJobRequest, TriggeredJobWithTasksModel> StartJob { get; }
    public AppApiActionBuilder<StartTaskRequest, EmptyActionResult> StartTask { get; }
    public AppApiActionBuilder<TaskCompletedRequest, TriggeredJobWithTasksModel> TaskCompleted { get; }
    public AppApiActionBuilder<TaskFailedRequest, TriggeredJobWithTasksModel> TaskFailed { get; }
    public AppApiActionBuilder<TriggerJobsRequest, PendingJobModel[]> TriggerJobs { get; }

    public JobsGroup Build() => new JobsGroup(source, this);
}