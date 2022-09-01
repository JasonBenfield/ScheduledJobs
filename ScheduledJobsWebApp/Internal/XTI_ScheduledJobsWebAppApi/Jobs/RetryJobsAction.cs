namespace XTI_ScheduledJobsWebAppApi.Jobs;

internal sealed class RetryJobsAction : AppAction<RetryJobsRequest, TriggeredJobWithTasksModel[]>
{
    private readonly IJobDb db;

    public RetryJobsAction(IJobDb db)
    {
        this.db = db;
    }

    public Task<TriggeredJobWithTasksModel[]> Execute(RetryJobsRequest model, CancellationToken ct) =>
        db.RetryJobs(model.EventKey, model.JobKey);
}
