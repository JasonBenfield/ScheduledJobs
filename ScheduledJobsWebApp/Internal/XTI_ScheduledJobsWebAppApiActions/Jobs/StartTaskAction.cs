namespace XTI_ScheduledJobsWebAppApiActions.Jobs;

public sealed class StartTaskAction : AppAction<StartTaskRequest, EmptyActionResult>
{
    private readonly IJobDb db;

    public StartTaskAction(IJobDb db)
    {
        this.db = db;
    }

    public async Task<EmptyActionResult> Execute(StartTaskRequest model, CancellationToken ct)
    {
        await db.StartTask(model.TaskID);
        return new EmptyActionResult();
    }
}
