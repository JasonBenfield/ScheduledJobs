// Generated Code
namespace ScheduledJobsWebApp.ApiControllers;
[Authorize]
public class JobsController : Controller
{
    private readonly ScheduledJobsAppApi api;
    public JobsController(ScheduledJobsAppApi api)
    {
        this.api = api;
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
    public Task<ResultContainer<TriggeredJobDetailModel[]>> RetryJobs([FromBody] RetryJobsRequest model, CancellationToken ct)
    {
        return api.Group("Jobs").Action<RetryJobsRequest, TriggeredJobDetailModel[]>("RetryJobs").Execute(model, ct);
    }

    [HttpPost]
    public Task<ResultContainer<TriggeredJobDetailModel>> StartJob([FromBody] StartJobRequest model, CancellationToken ct)
    {
        return api.Group("Jobs").Action<StartJobRequest, TriggeredJobDetailModel>("StartJob").Execute(model, ct);
    }

    [HttpPost]
    public Task<ResultContainer<EmptyActionResult>> StartTask([FromBody] StartTaskRequest model, CancellationToken ct)
    {
        return api.Group("Jobs").Action<StartTaskRequest, EmptyActionResult>("StartTask").Execute(model, ct);
    }

    [HttpPost]
    public Task<ResultContainer<TriggeredJobDetailModel>> TaskCompleted([FromBody] TaskCompletedRequest model, CancellationToken ct)
    {
        return api.Group("Jobs").Action<TaskCompletedRequest, TriggeredJobDetailModel>("TaskCompleted").Execute(model, ct);
    }

    [HttpPost]
    public Task<ResultContainer<TriggeredJobDetailModel>> TaskFailed([FromBody] TaskFailedRequest model, CancellationToken ct)
    {
        return api.Group("Jobs").Action<TaskFailedRequest, TriggeredJobDetailModel>("TaskFailed").Execute(model, ct);
    }

    [HttpPost]
    public Task<ResultContainer<EmptyActionResult>> LogMessage([FromBody] LogMessageRequest model, CancellationToken ct)
    {
        return api.Group("Jobs").Action<LogMessageRequest, EmptyActionResult>("LogMessage").Execute(model, ct);
    }
}