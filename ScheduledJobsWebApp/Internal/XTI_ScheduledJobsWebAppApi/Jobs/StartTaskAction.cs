namespace XTI_ScheduledJobsWebAppApi.Jobs;

internal sealed class StartTaskAction : AppAction<StartTaskRequest, EmptyActionResult>
{
    private readonly IJobDb db;

    public StartTaskAction(IJobDb db)
    {
        this.db = db;
    }

    public async Task<EmptyActionResult> Execute(StartTaskRequest model)
    {
        await db.StartTask(model.TaskID);
        return new EmptyActionResult();
    }
}
