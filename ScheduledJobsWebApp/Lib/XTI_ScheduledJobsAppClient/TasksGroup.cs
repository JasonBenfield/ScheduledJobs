// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class TasksGroup : AppClientGroup
{
    public TasksGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl, AppClientOptions options) : base(httpClientFactory, xtiTokenAccessor, clientUrl, options, "Tasks")
    {
        Actions = new TasksGroupActions(CancelTask: CreatePostAction<GetTaskRequest, EmptyActionResult>("CancelTask"), RetryTask: CreatePostAction<GetTaskRequest, EmptyActionResult>("RetryTask"), SkipTask: CreatePostAction<GetTaskRequest, EmptyActionResult>("SkipTask"), TimeoutTask: CreatePostAction<GetTaskRequest, EmptyActionResult>("TimeoutTask"), EditTaskData: CreatePostAction<EditTaskDataRequest, EmptyActionResult>("EditTaskData"));
    }

    public TasksGroupActions Actions { get; }

    public Task<EmptyActionResult> CancelTask(GetTaskRequest model, CancellationToken ct = default) => Actions.CancelTask.Post("", model, ct);
    public Task<EmptyActionResult> RetryTask(GetTaskRequest model, CancellationToken ct = default) => Actions.RetryTask.Post("", model, ct);
    public Task<EmptyActionResult> SkipTask(GetTaskRequest model, CancellationToken ct = default) => Actions.SkipTask.Post("", model, ct);
    public Task<EmptyActionResult> TimeoutTask(GetTaskRequest model, CancellationToken ct = default) => Actions.TimeoutTask.Post("", model, ct);
    public Task<EmptyActionResult> EditTaskData(EditTaskDataRequest model, CancellationToken ct = default) => Actions.EditTaskData.Post("", model, ct);
    public sealed record TasksGroupActions(AppClientPostAction<GetTaskRequest, EmptyActionResult> CancelTask, AppClientPostAction<GetTaskRequest, EmptyActionResult> RetryTask, AppClientPostAction<GetTaskRequest, EmptyActionResult> SkipTask, AppClientPostAction<GetTaskRequest, EmptyActionResult> TimeoutTask, AppClientPostAction<EditTaskDataRequest, EmptyActionResult> EditTaskData);
}