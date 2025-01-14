// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class TasksGroup : AppClientGroup
{
    public TasksGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl, AppClientOptions options) : base(httpClientFactory, xtiTokenAccessor, clientUrl, options, "Tasks")
    {
        Actions = new TasksGroupActions(CancelTask: CreatePostAction<GetTaskRequest, EmptyActionResult>("CancelTask"), EditTaskData: CreatePostAction<EditTaskDataRequest, EmptyActionResult>("EditTaskData"), RetryTask: CreatePostAction<GetTaskRequest, EmptyActionResult>("RetryTask"), SkipTask: CreatePostAction<GetTaskRequest, EmptyActionResult>("SkipTask"), TimeoutTask: CreatePostAction<GetTaskRequest, EmptyActionResult>("TimeoutTask"));
        Configure();
    }

    partial void Configure();
    public TasksGroupActions Actions { get; }

    public Task<EmptyActionResult> CancelTask(GetTaskRequest requestData, CancellationToken ct = default) => Actions.CancelTask.Post("", requestData, ct);
    public Task<EmptyActionResult> EditTaskData(EditTaskDataRequest requestData, CancellationToken ct = default) => Actions.EditTaskData.Post("", requestData, ct);
    public Task<EmptyActionResult> RetryTask(GetTaskRequest requestData, CancellationToken ct = default) => Actions.RetryTask.Post("", requestData, ct);
    public Task<EmptyActionResult> SkipTask(GetTaskRequest requestData, CancellationToken ct = default) => Actions.SkipTask.Post("", requestData, ct);
    public Task<EmptyActionResult> TimeoutTask(GetTaskRequest requestData, CancellationToken ct = default) => Actions.TimeoutTask.Post("", requestData, ct);
    public sealed record TasksGroupActions(AppClientPostAction<GetTaskRequest, EmptyActionResult> CancelTask, AppClientPostAction<EditTaskDataRequest, EmptyActionResult> EditTaskData, AppClientPostAction<GetTaskRequest, EmptyActionResult> RetryTask, AppClientPostAction<GetTaskRequest, EmptyActionResult> SkipTask, AppClientPostAction<GetTaskRequest, EmptyActionResult> TimeoutTask);
}