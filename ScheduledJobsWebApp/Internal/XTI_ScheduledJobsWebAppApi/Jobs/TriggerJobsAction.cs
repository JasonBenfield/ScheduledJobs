namespace XTI_ScheduledJobsWebAppApi.Jobs;

internal sealed class TriggerJobsAction : AppAction<TriggerJobsRequest, PendingJobModel[]>
{
    private readonly IJobDb db;

    public TriggerJobsAction(IJobDb db)
    {
        this.db = db;
    }

    public Task<PendingJobModel[]> Execute(TriggerJobsRequest model) =>
        db.TriggerJobs(model.EventKey, model.JobKey, model.EventRaisedStartTime);
}
