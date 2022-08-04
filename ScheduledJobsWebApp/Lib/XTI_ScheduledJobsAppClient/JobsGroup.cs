// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class JobsGroup : AppClientGroup
{
    public JobsGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl, AppClientOptions options) : base(httpClientFactory, xtiTokenAccessor, clientUrl, options, "Jobs")
    {
        Actions = new JobsGroupActions(AddOrUpdateRegisteredJobs: CreatePostAction<RegisteredJob[], EmptyActionResult>("AddOrUpdateRegisteredJobs"), TriggerJobs: CreatePostAction<TriggerJobsRequest, PendingJobModel[]>("TriggerJobs"), RetryJobs: CreatePostAction<RetryJobsRequest, TriggeredJobWithTasksModel[]>("RetryJobs"), StartJob: CreatePostAction<StartJobRequest, TriggeredJobWithTasksModel>("StartJob"), StartTask: CreatePostAction<StartTaskRequest, EmptyActionResult>("StartTask"), JobCancelled: CreatePostAction<JobCancelledRequest, EmptyActionResult>("JobCancelled"), TaskCompleted: CreatePostAction<TaskCompletedRequest, TriggeredJobWithTasksModel>("TaskCompleted"), TaskFailed: CreatePostAction<TaskFailedRequest, TriggeredJobWithTasksModel>("TaskFailed"), LogMessage: CreatePostAction<LogMessageRequest, EmptyActionResult>("LogMessage"));
    }

    public JobsGroupActions Actions { get; }

    public Task<EmptyActionResult> AddOrUpdateRegisteredJobs(RegisteredJob[] model) => Actions.AddOrUpdateRegisteredJobs.Post("", model);
    public Task<PendingJobModel[]> TriggerJobs(TriggerJobsRequest model) => Actions.TriggerJobs.Post("", model);
    public Task<TriggeredJobWithTasksModel[]> RetryJobs(RetryJobsRequest model) => Actions.RetryJobs.Post("", model);
    public Task<TriggeredJobWithTasksModel> StartJob(StartJobRequest model) => Actions.StartJob.Post("", model);
    public Task<EmptyActionResult> StartTask(StartTaskRequest model) => Actions.StartTask.Post("", model);
    public Task<EmptyActionResult> JobCancelled(JobCancelledRequest model) => Actions.JobCancelled.Post("", model);
    public Task<TriggeredJobWithTasksModel> TaskCompleted(TaskCompletedRequest model) => Actions.TaskCompleted.Post("", model);
    public Task<TriggeredJobWithTasksModel> TaskFailed(TaskFailedRequest model) => Actions.TaskFailed.Post("", model);
    public Task<EmptyActionResult> LogMessage(LogMessageRequest model) => Actions.LogMessage.Post("", model);
    public sealed record JobsGroupActions(AppClientPostAction<RegisteredJob[], EmptyActionResult> AddOrUpdateRegisteredJobs, AppClientPostAction<TriggerJobsRequest, PendingJobModel[]> TriggerJobs, AppClientPostAction<RetryJobsRequest, TriggeredJobWithTasksModel[]> RetryJobs, AppClientPostAction<StartJobRequest, TriggeredJobWithTasksModel> StartJob, AppClientPostAction<StartTaskRequest, EmptyActionResult> StartTask, AppClientPostAction<JobCancelledRequest, EmptyActionResult> JobCancelled, AppClientPostAction<TaskCompletedRequest, TriggeredJobWithTasksModel> TaskCompleted, AppClientPostAction<TaskFailedRequest, TriggeredJobWithTasksModel> TaskFailed, AppClientPostAction<LogMessageRequest, EmptyActionResult> LogMessage);
}