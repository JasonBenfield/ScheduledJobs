namespace XTI_ScheduledJobsWebAppApi.Tasks;

public sealed class TasksGroup : AppApiGroupWrapper
{
    public TasksGroup(AppApiGroup source, IServiceProvider sp)
        : base(source)
    {
        CancelTask = source.AddAction
        (
            nameof(CancelTask), 
            () => sp.GetRequiredService<CancelTaskAction>()
        );
        RetryTask = source.AddAction
        (
            nameof(RetryTask), 
            () => sp.GetRequiredService<RetryTaskAction>()
        );
        TimeoutTask = source.AddAction
        (
            nameof(TimeoutTask),
            () => sp.GetRequiredService<TimeoutTaskAction>()
        );
        EditTaskData = source.AddAction
        (
            nameof(EditTaskData),
            () => sp.GetRequiredService<EditTaskDataAction>()
        );
    }

    public AppApiAction<GetTaskRequest, EmptyActionResult> CancelTask { get; }
    public AppApiAction<GetTaskRequest, EmptyActionResult> RetryTask { get; }
    public AppApiAction<GetTaskRequest, EmptyActionResult> TimeoutTask { get; }
    public AppApiAction<EditTaskDataRequest, EmptyActionResult> EditTaskData { get; }
}