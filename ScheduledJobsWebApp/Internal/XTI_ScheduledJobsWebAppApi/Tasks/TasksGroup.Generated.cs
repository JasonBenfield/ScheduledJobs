using XTI_ScheduledJobsWebAppApiActions.Tasks;

// Generated Code
#nullable enable
namespace XTI_ScheduledJobsWebAppApi.Tasks;
public sealed partial class TasksGroup : AppApiGroupWrapper
{
    internal TasksGroup(AppApiGroup source, TasksGroupBuilder builder) : base(source)
    {
        CancelTask = builder.CancelTask.Build();
        EditTaskData = builder.EditTaskData.Build();
        RetryTask = builder.RetryTask.Build();
        SkipTask = builder.SkipTask.Build();
        TimeoutTask = builder.TimeoutTask.Build();
        Configure();
    }

    partial void Configure();
    public AppApiAction<GetTaskRequest, EmptyActionResult> CancelTask { get; }
    public AppApiAction<EditTaskDataRequest, EmptyActionResult> EditTaskData { get; }
    public AppApiAction<GetTaskRequest, EmptyActionResult> RetryTask { get; }
    public AppApiAction<GetTaskRequest, EmptyActionResult> SkipTask { get; }
    public AppApiAction<GetTaskRequest, EmptyActionResult> TimeoutTask { get; }
}