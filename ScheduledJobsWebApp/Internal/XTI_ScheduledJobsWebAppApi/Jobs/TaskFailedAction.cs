namespace XTI_ScheduledJobsWebAppApi.Jobs;

internal sealed class TaskFailedAction : AppAction<TaskFailedRequest, TriggeredJobDetailModel>
{
    private readonly IJobDb db;

    public TaskFailedAction(IJobDb db)
    {
        this.db = db;
    }

    public Task<TriggeredJobDetailModel> Execute(TaskFailedRequest model, CancellationToken ct) =>
        db.TaskFailed
        (
            model.JobID,
            model.FailedTaskID,
            model.ErrorStatus,
            model.RetryAfter,
            model.NextTasks,
            model.Category,
            model.Message,
            model.Detail
        );
}
