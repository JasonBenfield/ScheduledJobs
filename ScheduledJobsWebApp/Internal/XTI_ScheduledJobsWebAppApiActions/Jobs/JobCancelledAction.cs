namespace XTI_ScheduledJobsWebAppApiActions.Jobs;

public sealed class JobCancelledAction : AppAction<JobCancelledRequest, EmptyActionResult>
{
    private readonly IJobDb db;

    public JobCancelledAction(IJobDb db)
    {
        this.db = db;
    }

    public async Task<EmptyActionResult> Execute(JobCancelledRequest model, CancellationToken stoppingToken)
    {
        await db.JobCancelled(model.TaskID, model.Reason);
        return new EmptyActionResult();
    }
}
