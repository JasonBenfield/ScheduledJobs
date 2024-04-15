namespace XTI_ScheduledJobsWebAppApi.Jobs;

internal sealed class TaskFailedAction : AppAction<TaskFailedRequest, TriggeredJobWithTasksModel>
{
    private readonly IJobDb db;

    public TaskFailedAction(IJobDb db)
    {
        this.db = db;
    }

    public Task<TriggeredJobWithTasksModel> Execute(TaskFailedRequest failedRequest, CancellationToken ct) =>
        db.TaskFailed
        (
            failedRequest.FailedTaskID,
            JobTaskStatus.Values.Value(failedRequest.ErrorStatus),
            failedRequest.RetryAfter,
            failedRequest.NextTasks,
            failedRequest.Category,
            failedRequest.Message,
            failedRequest.Detail,
            failedRequest.SourceLogEntryKey
        );
}
