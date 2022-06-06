// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class TasksGroup : AppClientGroup
{
    public TasksGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl) : base(httpClientFactory, xtiTokenAccessor, clientUrl, "Tasks")
    {
    }

    public Task<EmptyActionResult> CancelTask(GetTaskRequest model) => Post<EmptyActionResult, GetTaskRequest>("CancelTask", "", model);
    public Task<EmptyActionResult> RetryTask(GetTaskRequest model) => Post<EmptyActionResult, GetTaskRequest>("RetryTask", "", model);
}