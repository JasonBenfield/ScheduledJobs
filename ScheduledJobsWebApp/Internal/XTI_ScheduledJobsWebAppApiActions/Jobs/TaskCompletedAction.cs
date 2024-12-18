namespace XTI_ScheduledJobsWebAppApiActions.Jobs;

public class TaskCompletedAction : AppAction<TaskCompletedRequest, TriggeredJobWithTasksModel>
{
    private readonly IJobDb db;

    public TaskCompletedAction(IJobDb db)
    {
        this.db = db;
    }

    public Task<TriggeredJobWithTasksModel> Execute(TaskCompletedRequest model, CancellationToken ct) =>
        db.TaskCompleted(model.CompletedTaskID, model.PreserveData, model.NextTasks);
}
