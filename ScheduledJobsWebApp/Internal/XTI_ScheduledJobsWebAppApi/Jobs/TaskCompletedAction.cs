namespace XTI_ScheduledJobsWebAppApi.Jobs;

internal class TaskCompletedAction : AppAction<TaskCompletedRequest, TriggeredJobDetailModel>
{
    private readonly IJobDb db;

    public TaskCompletedAction(IJobDb db)
    {
        this.db = db;
    }

    public Task<TriggeredJobDetailModel> Execute(TaskCompletedRequest model) =>
        db.TaskCompleted(model.JobID, model.CompletedTaskID, model.PreserveData, model.NextTasks);
}
