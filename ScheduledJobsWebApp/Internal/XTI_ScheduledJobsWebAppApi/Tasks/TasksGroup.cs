namespace XTI_ScheduledJobsWebAppApi.Tasks;

public sealed class TasksGroup : AppApiGroupWrapper
{
    public TasksGroup(AppApiGroup source, IServiceProvider sp)
        : base(source)
    {
        var actions = new AppApiActionFactory(source);
        CancelTask = source.AddAction
        (
            actions.Action(nameof(CancelTask), () => sp.GetRequiredService<CancelTaskAction>())
        );
        RetryTask = source.AddAction
        (
            actions.Action(nameof(RetryTask), () => sp.GetRequiredService<RetryTaskAction>())
        );
    }

    public AppApiAction<GetTaskRequest, EmptyActionResult> CancelTask { get; }
    public AppApiAction<GetTaskRequest, EmptyActionResult> RetryTask { get; }
}