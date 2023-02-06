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
    public Task<ResultContainer<EmptyActionResult>> AddOrUpdateJobSchedules([FromBody] AddOrUpdateJobSchedulesRequest model, CancellationToken ct)
    {
        return api.Group("Jobs").Action<AddOrUpdateJobSchedulesRequest, EmptyActionResult>("AddOrUpdateJobSchedules").Execute(model, ct);
    }

    [HttpPost]
    public Task<ResultContainer<EmptyActionResult>> AddOrUpdateRegisteredJobs([FromBody] RegisteredJob[] model, CancellationToken ct)
    {
        return api.Group("Jobs").Action<RegisteredJob[], EmptyActionResult>("AddOrUpdateRegisteredJobs").Execute(model, ct);
    }

    [HttpPost]
    public Task<ResultContainer<PendingJobModel[]>> TriggerJobs([FromBody] TriggerJobsRequest model, CancellationToken ct)
    {
        return api.Group("Jobs").Action<TriggerJobsRequest, PendingJobModel[]>("TriggerJobs").Execute(model, ct);
    }

    [HttpPost]
    public Task<ResultContainer<EmptyActionResult>> DeleteJobsWithNoTasks([FromBody] DeleteJobsWithNoTasksRequest model, CancellationToken ct)
    {
        return api.Group("Jobs").Action<DeleteJobsWithNoTasksRequest, EmptyActionResult>("DeleteJobsWithNoTasks").Execute(model, ct);
    }

    [HttpPost]
    public Task<ResultContainer<TriggeredJobWithTasksModel[]>> RetryJobs([FromBody] RetryJobsRequest model, CancellationToken ct)
    {
        return api.Group("Jobs").Action<RetryJobsRequest, TriggeredJobWithTasksModel[]>("RetryJobs").Execute(model, ct);
    }

    [HttpPost]
    public Task<ResultContainer<TriggeredJobWithTasksModel>> StartJob([FromBody] StartJobRequest model, CancellationToken ct)
    {
        return api.Group("Jobs").Action<StartJobRequest, TriggeredJobWithTasksModel>("StartJob").Execute(model, ct);
    }

    [HttpPost]
    public Task<ResultContainer<EmptyActionResult>> StartTask([FromBody] StartTaskRequest model, CancellationToken ct)
    {
        return api.Group("Jobs").Action<StartTaskRequest, EmptyActionResult>("StartTask").Execute(model, ct);
    }

    [HttpPost]
    public Task<ResultContainer<EmptyActionResult>> JobCancelled([FromBody] JobCancelledRequest model, CancellationToken ct)
    {
        return api.Group("Jobs").Action<JobCancelledRequest, EmptyActionResult>("JobCancelled").Execute(model, ct);
    }

    [HttpPost]
    public Task<ResultContainer<TriggeredJobWithTasksModel>> TaskCompleted([FromBody] TaskCompletedRequest model, CancellationToken ct)
    {
        return api.Group("Jobs").Action<TaskCompletedRequest, TriggeredJobWithTasksModel>("TaskCompleted").Execute(model, ct);
    }

    [HttpPost]
    public Task<ResultContainer<TriggeredJobWithTasksModel>> TaskFailed([FromBody] TaskFailedRequest model, CancellationToken ct)
    {
        return api.Group("Jobs").Action<TaskFailedRequest, TriggeredJobWithTasksModel>("TaskFailed").Execute(model, ct);
    }

    [HttpPost]
    public Task<ResultContainer<EmptyActionResult>> LogMessage([FromBody] LogMessageRequest model, CancellationToken ct)
    {
        return api.Group("Jobs").Action<LogMessageRequest, EmptyActionResult>("LogMessage").Execute(model, ct);
    }
}