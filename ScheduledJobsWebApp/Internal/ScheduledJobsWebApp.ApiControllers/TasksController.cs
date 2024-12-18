// Generated Code
namespace ScheduledJobsWebApp.ApiControllers;
[Authorize]
public sealed partial class TasksController : Controller
{
    private readonly ScheduledJobsAppApi api;
    public TasksController(ScheduledJobsAppApi api)
    {
        this.api = api;
    }

    [HttpPost]
    public Task<ResultContainer<EmptyActionResult>> CancelTask([FromBody] GetTaskRequest requestData, CancellationToken ct)
    {
        return api.Tasks.CancelTask.Execute(requestData, ct);
    }

    [HttpPost]
    public Task<ResultContainer<EmptyActionResult>> EditTaskData([FromBody] EditTaskDataRequest requestData, CancellationToken ct)
    {
        return api.Tasks.EditTaskData.Execute(requestData, ct);
    }

    [HttpPost]
    public Task<ResultContainer<EmptyActionResult>> RetryTask([FromBody] GetTaskRequest requestData, CancellationToken ct)
    {
        return api.Tasks.RetryTask.Execute(requestData, ct);
    }

    [HttpPost]
    public Task<ResultContainer<EmptyActionResult>> SkipTask([FromBody] GetTaskRequest requestData, CancellationToken ct)
    {
        return api.Tasks.SkipTask.Execute(requestData, ct);
    }

    [HttpPost]
    public Task<ResultContainer<EmptyActionResult>> TimeoutTask([FromBody] GetTaskRequest requestData, CancellationToken ct)
    {
        return api.Tasks.TimeoutTask.Execute(requestData, ct);
    }
}