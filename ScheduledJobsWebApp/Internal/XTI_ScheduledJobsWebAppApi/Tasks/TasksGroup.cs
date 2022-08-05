namespace XTI_ScheduledJobsWebAppApi.Tasks;

public sealed class TasksGroup : AppApiGroupWrapper
{
    public TasksGroup(AppApiGroup source, IServiceProvider sp)
        : base(source)
    {
        CancelTask = source.AddAction
        (
            nameof(CancelTask), () => sp.GetRequiredService<CancelTaskAction>()
        );
        RetryTask = source.AddAction
        (
            nameof(RetryTask), () => sp.GetRequiredService<RetryTaskAction>()
        );
    }

    public AppApiAction<GetTaskRequest, EmptyActionResult> CancelTask { get; }
    public AppApiAction<GetTaskRequest, EmptyActionResult> RetryTask { get; }
}