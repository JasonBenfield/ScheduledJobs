// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class JobsGroup : AppClientGroup
{
    public JobsGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl, AppClientOptions options) : base(httpClientFactory, xtiTokenAccessor, clientUrl, options, "Jobs")
    {
        Actions = new JobsGroupActions(AddOrUpdateJobSchedules: CreatePostAction<AddOrUpdateJobSchedulesRequest, EmptyActionResult>("AddOrUpdateJobSchedules"), AddOrUpdateRegisteredJobs: CreatePostAction<RegisteredJob[], EmptyActionResult>("AddOrUpdateRegisteredJobs"), TriggerJobs: CreatePostAction<TriggerJobsRequest, PendingJobModel[]>("TriggerJobs"), DeleteJobsWithNoTasks: CreatePostAction<DeleteJobsWithNoTasksRequest, EmptyActionResult>("DeleteJobsWithNoTasks"), RetryJobs: CreatePostAction<RetryJobsRequest, TriggeredJobWithTasksModel[]>("RetryJobs"), StartJob: CreatePostAction<StartJobRequest, TriggeredJobWithTasksModel>("StartJob"), StartTask: CreatePostAction<StartTaskRequest, EmptyActionResult>("StartTask"), JobCancelled: CreatePostAction<JobCancelledRequest, EmptyActionResult>("JobCancelled"), TaskCompleted: CreatePostAction<TaskCompletedRequest, TriggeredJobWithTasksModel>("TaskCompleted"), TaskFailed: CreatePostAction<TaskFailedRequest, TriggeredJobWithTasksModel>("TaskFailed"), LogMessage: CreatePostAction<LogMessageRequest, EmptyActionResult>("LogMessage"));
    }

    public JobsGroupActions Actions { get; }

    public Task<EmptyActionResult> AddOrUpdateJobSchedules(AddOrUpdateJobSchedulesRequest model, CancellationToken ct = default) => Actions.AddOrUpdateJobSchedules.Post("", model, ct);
    public Task<EmptyActionResult> AddOrUpdateRegisteredJobs(RegisteredJob[] model, CancellationToken ct = default) => Actions.AddOrUpdateRegisteredJobs.Post("", model, ct);
    public Task<PendingJobModel[]> TriggerJobs(TriggerJobsRequest model, CancellationToken ct = default) => Actions.TriggerJobs.Post("", model, ct);
    public Task<EmptyActionResult> DeleteJobsWithNoTasks(DeleteJobsWithNoTasksRequest model, CancellationToken ct = default) => Actions.DeleteJobsWithNoTasks.Post("", model, ct);
    public Task<TriggeredJobWithTasksModel[]> RetryJobs(RetryJobsRequest model, CancellationToken ct = default) => Actions.RetryJobs.Post("", model, ct);
    public Task<TriggeredJobWithTasksModel> StartJob(StartJobRequest model, CancellationToken ct = default) => Actions.StartJob.Post("", model, ct);
    public Task<EmptyActionResult> StartTask(StartTaskRequest model, CancellationToken ct = default) => Actions.StartTask.Post("", model, ct);
    public Task<EmptyActionResult> JobCancelled(JobCancelledRequest model, CancellationToken ct = default) => Actions.JobCancelled.Post("", model, ct);
    public Task<TriggeredJobWithTasksModel> TaskCompleted(TaskCompletedRequest model, CancellationToken ct = default) => Actions.TaskCompleted.Post("", model, ct);
    public Task<TriggeredJobWithTasksModel> TaskFailed(TaskFailedRequest model, CancellationToken ct = default) => Actions.TaskFailed.Post("", model, ct);
    public Task<EmptyActionResult> LogMessage(LogMessageRequest model, CancellationToken ct = default) => Actions.LogMessage.Post("", model, ct);
    public sealed record JobsGroupActions(AppClientPostAction<AddOrUpdateJobSchedulesRequest, EmptyActionResult> AddOrUpdateJobSchedules, AppClientPostAction<RegisteredJob[], EmptyActionResult> AddOrUpdateRegisteredJobs, AppClientPostAction<TriggerJobsRequest, PendingJobModel[]> TriggerJobs, AppClientPostAction<DeleteJobsWithNoTasksRequest, EmptyActionResult> DeleteJobsWithNoTasks, AppClientPostAction<RetryJobsRequest, TriggeredJobWithTasksModel[]> RetryJobs, AppClientPostAction<StartJobRequest, TriggeredJobWithTasksModel> StartJob, AppClientPostAction<StartTaskRequest, EmptyActionResult> StartTask, AppClientPostAction<JobCancelledRequest, EmptyActionResult> JobCancelled, AppClientPostAction<TaskCompletedRequest, TriggeredJobWithTasksModel> TaskCompleted, AppClientPostAction<TaskFailedRequest, TriggeredJobWithTasksModel> TaskFailed, AppClientPostAction<LogMessageRequest, EmptyActionResult> LogMessage);
}