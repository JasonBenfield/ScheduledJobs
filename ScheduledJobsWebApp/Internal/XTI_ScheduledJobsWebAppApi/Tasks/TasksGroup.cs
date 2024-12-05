namespace XTI_ScheduledJobsWebAppApi.Tasks;

public sealed class TasksGroup : AppApiGroupWrapper
{
    public TasksGroup(AppApiGroup source, IServiceProvider sp)
        : base(source)
    {
        CancelTask = source.AddAction<GetTaskRequest, EmptyActionResult>()
            .Named(nameof(CancelTask))
            .WithExecution<CancelTaskAction>()
            .Build();
        RetryTask = source.AddAction<GetTaskRequest, EmptyActionResult>()
            .Named(nameof(RetryTask))
            .WithExecution<RetryTaskAction>()
            .Build();
        SkipTask = source.AddAction<GetTaskRequest, EmptyActionResult>()
            .Named(nameof(SkipTask))
            .WithExecution<SkipTaskAction>()
            .Build();
        TimeoutTask = source.AddAction<GetTaskRequest, EmptyActionResult>()
            .Named(nameof(TimeoutTask))
            .WithExecution<TimeoutTaskAction>()
            .Build();
        EditTaskData = source.AddAction<EditTaskDataRequest, EmptyActionResult>()
            .Named(nameof(EditTaskData))
            .WithExecution<EditTaskDataAction>()
            .Build();
    }

    public AppApiAction<GetTaskRequest, EmptyActionResult> CancelTask { get; }
    public AppApiAction<GetTaskRequest, EmptyActionResult> RetryTask { get; }
    public AppApiAction<GetTaskRequest, EmptyActionResult> SkipTask { get; }
    public AppApiAction<GetTaskRequest, EmptyActionResult> TimeoutTask { get; }
    public AppApiAction<EditTaskDataRequest, EmptyActionResult> EditTaskData { get; }
}