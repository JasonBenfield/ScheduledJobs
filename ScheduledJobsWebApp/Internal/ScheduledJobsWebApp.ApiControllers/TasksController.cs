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
    public Task<ResultContainer<EmptyActionResult>> CancelTask([FromBody] GetTaskRequest model, CancellationToken ct)
    {
        return api.Group("Tasks").Action<GetTaskRequest, EmptyActionResult>("CancelTask").Execute(model, ct);
    }

    [HttpPost]
    public Task<ResultContainer<EmptyActionResult>> RetryTask([FromBody] GetTaskRequest model, CancellationToken ct)
    {
        return api.Group("Tasks").Action<GetTaskRequest, EmptyActionResult>("RetryTask").Execute(model, ct);
    }

    [HttpPost]
    public Task<ResultContainer<EmptyActionResult>> TimeoutTask([FromBody] GetTaskRequest model, CancellationToken ct)
    {
        return api.Group("Tasks").Action<GetTaskRequest, EmptyActionResult>("TimeoutTask").Execute(model, ct);
    }

    [HttpPost]
    public Task<ResultContainer<EmptyActionResult>> EditTaskData([FromBody] EditTaskDataRequest model, CancellationToken ct)
    {
        return api.Group("Tasks").Action<EditTaskDataRequest, EmptyActionResult>("EditTaskData").Execute(model, ct);
    }
}