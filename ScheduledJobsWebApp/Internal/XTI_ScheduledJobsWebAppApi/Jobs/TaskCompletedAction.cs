namespace XTI_ScheduledJobsWebAppApi.Jobs;

internal class TaskCompletedAction : AppAction<TaskCompletedRequest, TriggeredJobWithTasksModel>
{
    private readonly IJobDb db;

    public TaskCompletedAction(IJobDb db)
    {
        this.db = db;
    }

    public Task<TriggeredJobWithTasksModel> Execute(TaskCompletedRequest model, CancellationToken ct) =>
        db.TaskCompleted(model.JobID, model.CompletedTaskID, model.PreserveData, model.NextTasks);
}
