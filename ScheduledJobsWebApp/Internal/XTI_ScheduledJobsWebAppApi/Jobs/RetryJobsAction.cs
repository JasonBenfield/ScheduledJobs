namespace XTI_ScheduledJobsWebAppApi.Jobs;

internal sealed class RetryJobsAction : AppAction<RetryJobsRequest, TriggeredJobDetailModel[]>
{
    private readonly IJobDb db;

    public RetryJobsAction(IJobDb db)
    {
        this.db = db;
    }

    public Task<TriggeredJobDetailModel[]> Execute(RetryJobsRequest model) =>
        db.RetryJobs(model.JobKey);
}
