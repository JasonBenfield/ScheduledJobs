// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class TasksGroup : AppClientGroup
{
    public TasksGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl, AppClientOptions options) : base(httpClientFactory, xtiTokenAccessor, clientUrl, options, "Tasks")
    {
        Actions = new TasksGroupActions(CancelTask: CreatePostAction<GetTaskRequest, EmptyActionResult>("CancelTask"), RetryTask: CreatePostAction<GetTaskRequest, EmptyActionResult>("RetryTask"));
    }

    public TasksGroupActions Actions { get; }

    public Task<EmptyActionResult> CancelTask(GetTaskRequest model, CancellationToken ct = default) => Actions.CancelTask.Post("", model, ct);
    public Task<EmptyActionResult> RetryTask(GetTaskRequest model, CancellationToken ct = default) => Actions.RetryTask.Post("", model, ct);
    public sealed record TasksGroupActions(AppClientPostAction<GetTaskRequest, EmptyActionResult> CancelTask, AppClientPostAction<GetTaskRequest, EmptyActionResult> RetryTask);
}