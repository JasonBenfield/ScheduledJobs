// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class JobsGroup : AppClientGroup
{
    public JobsGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl, AppClientOptions options) : base(httpClientFactory, xtiTokenAccessor, clientUrl, options, "Jobs")
    {
        Actions = new JobsGroupActions(AddOrUpdateJobSchedules: CreatePostAction<AddOrUpdateJobSchedulesRequest, EmptyActionResult>("AddOrUpdateJobSchedules"), AddOrUpdateRegisteredJobs: CreatePostAction<RegisteredJob[], EmptyActionResult>("AddOrUpdateRegisteredJobs"), DeleteJobsWithNoTasks: CreatePostAction<DeleteJobsWithNoTasksRequest, EmptyActionResult>("DeleteJobsWithNoTasks"), JobCancelled: CreatePostAction<JobCancelledRequest, EmptyActionResult>("JobCancelled"), LogMessage: CreatePostAction<LogMessageRequest, EmptyActionResult>("LogMessage"), RetryJobs: CreatePostAction<RetryJobsRequest, TriggeredJobWithTasksModel[]>("RetryJobs"), StartJob: CreatePostAction<StartJobRequest, TriggeredJobWithTasksModel>("StartJob"), StartTask: CreatePostAction<StartTaskRequest, EmptyActionResult>("StartTask"), TaskCompleted: CreatePostAction<TaskCompletedRequest, TriggeredJobWithTasksModel>("TaskCompleted"), TaskFailed: CreatePostAction<TaskFailedRequest, TriggeredJobWithTasksModel>("TaskFailed"), TriggerJobs: CreatePostAction<TriggerJobsRequest, PendingJobModel[]>("TriggerJobs"));
    }

    public JobsGroupActions Actions { get; }

    public Task<EmptyActionResult> AddOrUpdateJobSchedules(AddOrUpdateJobSchedulesRequest requestData, CancellationToken ct = default) => Actions.AddOrUpdateJobSchedules.Post("", requestData, ct);
    public Task<EmptyActionResult> AddOrUpdateRegisteredJobs(RegisteredJob[] requestData, CancellationToken ct = default) => Actions.AddOrUpdateRegisteredJobs.Post("", requestData, ct);
    public Task<EmptyActionResult> DeleteJobsWithNoTasks(DeleteJobsWithNoTasksRequest requestData, CancellationToken ct = default) => Actions.DeleteJobsWithNoTasks.Post("", requestData, ct);
    public Task<EmptyActionResult> JobCancelled(JobCancelledRequest requestData, CancellationToken ct = default) => Actions.JobCancelled.Post("", requestData, ct);
    public Task<EmptyActionResult> LogMessage(LogMessageRequest requestData, CancellationToken ct = default) => Actions.LogMessage.Post("", requestData, ct);
    public Task<TriggeredJobWithTasksModel[]> RetryJobs(RetryJobsRequest requestData, CancellationToken ct = default) => Actions.RetryJobs.Post("", requestData, ct);
    public Task<TriggeredJobWithTasksModel> StartJob(StartJobRequest requestData, CancellationToken ct = default) => Actions.StartJob.Post("", requestData, ct);
    public Task<EmptyActionResult> StartTask(StartTaskRequest requestData, CancellationToken ct = default) => Actions.StartTask.Post("", requestData, ct);
    public Task<TriggeredJobWithTasksModel> TaskCompleted(TaskCompletedRequest requestData, CancellationToken ct = default) => Actions.TaskCompleted.Post("", requestData, ct);
    public Task<TriggeredJobWithTasksModel> TaskFailed(TaskFailedRequest requestData, CancellationToken ct = default) => Actions.TaskFailed.Post("", requestData, ct);
    public Task<PendingJobModel[]> TriggerJobs(TriggerJobsRequest requestData, CancellationToken ct = default) => Actions.TriggerJobs.Post("", requestData, ct);
    public sealed record JobsGroupActions(AppClientPostAction<AddOrUpdateJobSchedulesRequest, EmptyActionResult> AddOrUpdateJobSchedules, AppClientPostAction<RegisteredJob[], EmptyActionResult> AddOrUpdateRegisteredJobs, AppClientPostAction<DeleteJobsWithNoTasksRequest, EmptyActionResult> DeleteJobsWithNoTasks, AppClientPostAction<JobCancelledRequest, EmptyActionResult> JobCancelled, AppClientPostAction<LogMessageRequest, EmptyActionResult> LogMessage, AppClientPostAction<RetryJobsRequest, TriggeredJobWithTasksModel[]> RetryJobs, AppClientPostAction<StartJobRequest, TriggeredJobWithTasksModel> StartJob, AppClientPostAction<StartTaskRequest, EmptyActionResult> StartTask, AppClientPostAction<TaskCompletedRequest, TriggeredJobWithTasksModel> TaskCompleted, AppClientPostAction<TaskFailedRequest, TriggeredJobWithTasksModel> TaskFailed, AppClientPostAction<TriggerJobsRequest, PendingJobModel[]> TriggerJobs);
}