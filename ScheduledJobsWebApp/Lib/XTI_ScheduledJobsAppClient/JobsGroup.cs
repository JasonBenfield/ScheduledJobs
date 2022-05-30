// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class JobsGroup : AppClientGroup
{
    public JobsGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl) : base(httpClientFactory, xtiTokenAccessor, clientUrl, "Jobs")
    {
    }

    public Task<EmptyActionResult> AddOrUpdateRegisteredJobs(RegisteredJob[] model) => Post<EmptyActionResult, RegisteredJob[]>("AddOrUpdateRegisteredJobs", "", model);
    public Task<PendingJobModel[]> TriggerJobs(TriggerJobsRequest model) => Post<PendingJobModel[], TriggerJobsRequest>("TriggerJobs", "", model);
    public Task<TriggeredJobDetailModel[]> RetryJobs(RetryJobsRequest model) => Post<TriggeredJobDetailModel[], RetryJobsRequest>("RetryJobs", "", model);
    public Task<TriggeredJobDetailModel> StartJob(StartJobRequest model) => Post<TriggeredJobDetailModel, StartJobRequest>("StartJob", "", model);
    public Task<EmptyActionResult> StartTask(StartTaskRequest model) => Post<EmptyActionResult, StartTaskRequest>("StartTask", "", model);
    public Task<TriggeredJobDetailModel> TaskCompleted(TaskCompletedRequest model) => Post<TriggeredJobDetailModel, TaskCompletedRequest>("TaskCompleted", "", model);
    public Task<TriggeredJobDetailModel> TaskFailed(TaskFailedRequest model) => Post<TriggeredJobDetailModel, TaskFailedRequest>("TaskFailed", "", model);
    public Task<EmptyActionResult> LogMessage(LogMessageRequest model) => Post<EmptyActionResult, LogMessageRequest>("LogMessage", "", model);
}