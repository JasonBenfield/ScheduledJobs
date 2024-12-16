namespace XTI_ScheduledJobsWebAppApiActions.Jobs;

public sealed class RetryJobsAction : AppAction<RetryJobsRequest, TriggeredJobWithTasksModel[]>
{
    private readonly IJobDb db;

    public RetryJobsAction(IJobDb db)
    {
        this.db = db;
    }

    public Task<TriggeredJobWithTasksModel[]> Execute(RetryJobsRequest retryRequest, CancellationToken ct) =>
        db.RetryJobs(new EventKey(retryRequest.EventKey), new JobKey(retryRequest.JobKey));
}
