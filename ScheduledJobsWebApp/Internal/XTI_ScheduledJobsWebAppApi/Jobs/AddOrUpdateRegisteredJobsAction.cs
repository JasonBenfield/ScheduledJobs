namespace XTI_ScheduledJobsWebAppApi.Jobs;

public sealed class AddOrUpdateRegisteredJobsAction : AppAction<RegisteredJob[], EmptyActionResult>
{
    private readonly IJobDb db;

    public AddOrUpdateRegisteredJobsAction(IJobDb db)
    {
        this.db = db;
    }

    public async Task<EmptyActionResult> Execute(RegisteredJob[] registeredJobs, CancellationToken ct)
    {
        await db.AddOrUpdateRegisteredJobs(registeredJobs);
        return new EmptyActionResult();
    }
}