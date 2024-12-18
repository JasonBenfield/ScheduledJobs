using XTI_ScheduledJobsWebAppApiActions.Jobs;

// Generated Code
#nullable enable
namespace XTI_ScheduledJobsWebAppApi.Jobs;
public sealed partial class JobsGroup : AppApiGroupWrapper
{
    internal JobsGroup(AppApiGroup source, JobsGroupBuilder builder) : base(source)
    {
        AddOrUpdateJobSchedules = builder.AddOrUpdateJobSchedules.Build();
        AddOrUpdateRegisteredJobs = builder.AddOrUpdateRegisteredJobs.Build();
        DeleteJobsWithNoTasks = builder.DeleteJobsWithNoTasks.Build();
        JobCancelled = builder.JobCancelled.Build();
        LogMessage = builder.LogMessage.Build();
        RetryJobs = builder.RetryJobs.Build();
        StartJob = builder.StartJob.Build();
        StartTask = builder.StartTask.Build();
        TaskCompleted = builder.TaskCompleted.Build();
        TaskFailed = builder.TaskFailed.Build();
        TriggerJobs = builder.TriggerJobs.Build();
        Configure();
    }

    partial void Configure();
    public AppApiAction<AddOrUpdateJobSchedulesRequest, EmptyActionResult> AddOrUpdateJobSchedules { get; }
    public AppApiAction<RegisteredJob[], EmptyActionResult> AddOrUpdateRegisteredJobs { get; }
    public AppApiAction<DeleteJobsWithNoTasksRequest, EmptyActionResult> DeleteJobsWithNoTasks { get; }
    public AppApiAction<JobCancelledRequest, EmptyActionResult> JobCancelled { get; }
    public AppApiAction<LogMessageRequest, EmptyActionResult> LogMessage { get; }
    public AppApiAction<RetryJobsRequest, TriggeredJobWithTasksModel[]> RetryJobs { get; }
    public AppApiAction<StartJobRequest, TriggeredJobWithTasksModel> StartJob { get; }
    public AppApiAction<StartTaskRequest, EmptyActionResult> StartTask { get; }
    public AppApiAction<TaskCompletedRequest, TriggeredJobWithTasksModel> TaskCompleted { get; }
    public AppApiAction<TaskFailedRequest, TriggeredJobWithTasksModel> TaskFailed { get; }
    public AppApiAction<TriggerJobsRequest, PendingJobModel[]> TriggerJobs { get; }
}