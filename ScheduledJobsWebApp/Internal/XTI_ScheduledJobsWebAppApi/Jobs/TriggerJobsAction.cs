namespace XTI_ScheduledJobsWebAppApi.Jobs;

internal sealed class TriggerJobsAction : AppAction<TriggerJobsRequest, PendingJobModel[]>
{
    private readonly IJobDb db;

    public TriggerJobsAction(IJobDb db)
    {
        this.db = db;
    }

    public Task<PendingJobModel[]> Execute(TriggerJobsRequest triggerRequest, CancellationToken ct) =>
        db.TriggerJobs(new EventKey(triggerRequest.EventKey), new JobKey(triggerRequest.JobKey), triggerRequest.EventRaisedStartTime);
}
