namespace XTI_ScheduledJobsWebAppApiActions.Jobs;

public sealed class LogMessageAction : AppAction<LogMessageRequest, EmptyActionResult>
{
    private readonly IJobDb db;

    public LogMessageAction(IJobDb db)
    {
        this.db = db;
    }

    public async Task<EmptyActionResult> Execute(LogMessageRequest model, CancellationToken ct)
    {
        await db.LogMessage(model.TaskID, model.Category, model.Message, model.Details);
        return new EmptyActionResult();
    }
}
