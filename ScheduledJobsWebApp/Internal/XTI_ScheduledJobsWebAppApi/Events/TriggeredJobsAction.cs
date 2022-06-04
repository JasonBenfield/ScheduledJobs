namespace XTI_ScheduledJobsWebAppApi.Events;

internal sealed class TriggeredJobsAction : AppAction<TriggeredJobsRequest, TriggeredJobDetailModel[]>
{
    private readonly IJobDb db;

    public TriggeredJobsAction(IJobDb db)
    {
        this.db = db;
    }

    public Task<TriggeredJobDetailModel[]> Execute(TriggeredJobsRequest model, CancellationToken ct) =>
        db.TriggeredJobs(model.EventNotificationID);
}
