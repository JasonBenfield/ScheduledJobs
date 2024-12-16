// Generated Code
namespace ScheduledJobsWebApp.ApiControllers;
[Authorize]
public sealed partial class JobsController : Controller
{
    private readonly ScheduledJobsAppApi api;
    public JobsController(ScheduledJobsAppApi api)
    {
        this.api = api;
    }

    [HttpPost]
    public Task<ResultContainer<EmptyActionResult>> AddOrUpdateJobSchedules([FromBody] AddOrUpdateJobSchedulesRequest requestData, CancellationToken ct)
    {
        return api.Jobs.AddOrUpdateJobSchedules.Execute(requestData, ct);
    }

    [HttpPost]
    public Task<ResultContainer<EmptyActionResult>> AddOrUpdateRegisteredJobs([FromBody] RegisteredJob[] requestData, CancellationToken ct)
    {
        return api.Jobs.AddOrUpdateRegisteredJobs.Execute(requestData, ct);
    }

    [HttpPost]
    public Task<ResultContainer<EmptyActionResult>> DeleteJobsWithNoTasks([FromBody] DeleteJobsWithNoTasksRequest requestData, CancellationToken ct)
    {
        return api.Jobs.DeleteJobsWithNoTasks.Execute(requestData, ct);
    }

    [HttpPost]
    public Task<ResultContainer<EmptyActionResult>> JobCancelled([FromBody] JobCancelledRequest requestData, CancellationToken ct)
    {
        return api.Jobs.JobCancelled.Execute(requestData, ct);
    }

    [HttpPost]
    public Task<ResultContainer<EmptyActionResult>> LogMessage([FromBody] LogMessageRequest requestData, CancellationToken ct)
    {
        return api.Jobs.LogMessage.Execute(requestData, ct);
    }

    [HttpPost]
    public Task<ResultContainer<TriggeredJobWithTasksModel[]>> RetryJobs([FromBody] RetryJobsRequest requestData, CancellationToken ct)
    {
        return api.Jobs.RetryJobs.Execute(requestData, ct);
    }

    [HttpPost]
    public Task<ResultContainer<TriggeredJobWithTasksModel>> StartJob([FromBody] StartJobRequest requestData, CancellationToken ct)
    {
        return api.Jobs.StartJob.Execute(requestData, ct);
    }

    [HttpPost]
    public Task<ResultContainer<EmptyActionResult>> StartTask([FromBody] StartTaskRequest requestData, CancellationToken ct)
    {
        return api.Jobs.StartTask.Execute(requestData, ct);
    }

    [HttpPost]
    public Task<ResultContainer<TriggeredJobWithTasksModel>> TaskCompleted([FromBody] TaskCompletedRequest requestData, CancellationToken ct)
    {
        return api.Jobs.TaskCompleted.Execute(requestData, ct);
    }

    [HttpPost]
    public Task<ResultContainer<TriggeredJobWithTasksModel>> TaskFailed([FromBody] TaskFailedRequest requestData, CancellationToken ct)
    {
        return api.Jobs.TaskFailed.Execute(requestData, ct);
    }

    [HttpPost]
    public Task<ResultContainer<PendingJobModel[]>> TriggerJobs([FromBody] TriggerJobsRequest requestData, CancellationToken ct)
    {
        return api.Jobs.TriggerJobs.Execute(requestData, ct);
    }
}