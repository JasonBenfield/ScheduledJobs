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
    public Task<ResultContainer<EmptyActionResult>> AddOrUpdateRegisteredJobs([FromBody] RegisteredJob[] model)
    {
        return api.Group("Jobs").Action<RegisteredJob[], EmptyActionResult>("AddOrUpdateRegisteredJobs").Execute(model);
    }

    [HttpPost]
    public Task<ResultContainer<PendingJobModel[]>> TriggerJobs([FromBody] TriggerJobsRequest model)
    {
        return api.Group("Jobs").Action<TriggerJobsRequest, PendingJobModel[]>("TriggerJobs").Execute(model);
    }

    [HttpPost]
    public Task<ResultContainer<TriggeredJobDetailModel[]>> RetryJobs([FromBody] RetryJobsRequest model)
    {
        return api.Group("Jobs").Action<RetryJobsRequest, TriggeredJobDetailModel[]>("RetryJobs").Execute(model);
    }

    [HttpPost]
    public Task<ResultContainer<TriggeredJobDetailModel>> StartJob([FromBody] StartJobRequest model)
    {
        return api.Group("Jobs").Action<StartJobRequest, TriggeredJobDetailModel>("StartJob").Execute(model);
    }

    [HttpPost]
    public Task<ResultContainer<EmptyActionResult>> StartTask([FromBody] StartTaskRequest model)
    {
        return api.Group("Jobs").Action<StartTaskRequest, EmptyActionResult>("StartTask").Execute(model);
    }

    [HttpPost]
    public Task<ResultContainer<TriggeredJobDetailModel>> TaskCompleted([FromBody] TaskCompletedRequest model)
    {
        return api.Group("Jobs").Action<TaskCompletedRequest, TriggeredJobDetailModel>("TaskCompleted").Execute(model);
    }

    [HttpPost]
    public Task<ResultContainer<TriggeredJobDetailModel>> TaskFailed([FromBody] TaskFailedRequest model)
    {
        return api.Group("Jobs").Action<TaskFailedRequest, TriggeredJobDetailModel>("TaskFailed").Execute(model);
    }

    [HttpPost]
    public Task<ResultContainer<EmptyActionResult>> LogMessage([FromBody] LogMessageRequest model)
    {
        return api.Group("Jobs").Action<LogMessageRequest, EmptyActionResult>("LogMessage").Execute(model);
    }
}