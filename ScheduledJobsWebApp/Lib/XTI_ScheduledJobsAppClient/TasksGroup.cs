// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class TasksGroup : AppClientGroup
{
    public TasksGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl, AppClientOptions options) : base(httpClientFactory, xtiTokenAccessor, clientUrl, options, "Tasks")
    {
        Actions = new TasksGroupActions(CancelTask: CreatePostAction<GetTaskRequest, EmptyActionResult>("CancelTask"), RetryTask: CreatePostAction<GetTaskRequest, EmptyActionResult>("RetryTask"));
    }

    public TasksGroupActions Actions { get; }

    public Task<EmptyActionResult> CancelTask(GetTaskRequest model) => Actions.CancelTask.Post("", model);
    public Task<EmptyActionResult> RetryTask(GetTaskRequest model) => Actions.RetryTask.Post("", model);
    public sealed record TasksGroupActions(AppClientPostAction<GetTaskRequest, EmptyActionResult> CancelTask, AppClientPostAction<GetTaskRequest, EmptyActionResult> RetryTask);
}