using XTI_ScheduledJobsWebAppApiActions.Tasks;

// Generated Code
#nullable enable
namespace XTI_ScheduledJobsWebAppApi.Tasks;
public sealed partial class TasksGroupBuilder
{
    private readonly AppApiGroup source;
    internal TasksGroupBuilder(AppApiGroup source)
    {
        this.source = source;
        CancelTask = source.AddAction<GetTaskRequest, EmptyActionResult>("CancelTask").WithExecution<CancelTaskAction>();
        EditTaskData = source.AddAction<EditTaskDataRequest, EmptyActionResult>("EditTaskData").WithExecution<EditTaskDataAction>();
        RetryTask = source.AddAction<GetTaskRequest, EmptyActionResult>("RetryTask").WithExecution<RetryTaskAction>();
        SkipTask = source.AddAction<GetTaskRequest, EmptyActionResult>("SkipTask").WithExecution<SkipTaskAction>();
        TimeoutTask = source.AddAction<GetTaskRequest, EmptyActionResult>("TimeoutTask").WithExecution<TimeoutTaskAction>();
        Configure();
    }

    partial void Configure();
    public AppApiActionBuilder<GetTaskRequest, EmptyActionResult> CancelTask { get; }
    public AppApiActionBuilder<EditTaskDataRequest, EmptyActionResult> EditTaskData { get; }
    public AppApiActionBuilder<GetTaskRequest, EmptyActionResult> RetryTask { get; }
    public AppApiActionBuilder<GetTaskRequest, EmptyActionResult> SkipTask { get; }
    public AppApiActionBuilder<GetTaskRequest, EmptyActionResult> TimeoutTask { get; }

    public TasksGroup Build() => new TasksGroup(source, this);
}