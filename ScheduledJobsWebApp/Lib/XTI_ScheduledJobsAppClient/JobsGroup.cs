// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class JobsGroup : AppClientGroup
{
    public JobsGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl) : base(httpClientFactory, xtiTokenAccessor, clientUrl, "Jobs")
    {
    }

    public Task<EmptyActionResult> AddOrUpdateRegisteredJobs(RegisteredJob[] model) => Post<EmptyActionResult, RegisteredJob[]>("AddOrUpdateRegisteredJobs", "", model);
    public Task<PendingJobModel[]> TriggerJobs(TriggerJobsRequest model) => Post<PendingJobModel[], TriggerJobsRequest>("TriggerJobs", "", model);
    public Task<TriggeredJobWithTasksModel[]> RetryJobs(RetryJobsRequest model) => Post<TriggeredJobWithTasksModel[], RetryJobsRequest>("RetryJobs", "", model);
    public Task<TriggeredJobWithTasksModel> StartJob(StartJobRequest model) => Post<TriggeredJobWithTasksModel, StartJobRequest>("StartJob", "", model);
    public Task<EmptyActionResult> StartTask(StartTaskRequest model) => Post<EmptyActionResult, StartTaskRequest>("StartTask", "", model);
    public Task<EmptyActionResult> JobCancelled(JobCancelledRequest model) => Post<EmptyActionResult, JobCancelledRequest>("JobCancelled", "", model);
    public Task<TriggeredJobWithTasksModel> TaskCompleted(TaskCompletedRequest model) => Post<TriggeredJobWithTasksModel, TaskCompletedRequest>("TaskCompleted", "", model);
    public Task<TriggeredJobWithTasksModel> TaskFailed(TaskFailedRequest model) => Post<TriggeredJobWithTasksModel, TaskFailedRequest>("TaskFailed", "", model);
    public Task<EmptyActionResult> LogMessage(LogMessageRequest model) => Post<EmptyActionResult, LogMessageRequest>("LogMessage", "", model);
}