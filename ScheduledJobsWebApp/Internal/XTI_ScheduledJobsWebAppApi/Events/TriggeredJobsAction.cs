namespace XTI_ScheduledJobsWebAppApi.Events;

internal sealed class TriggeredJobsAction : AppAction<TriggeredJobsRequest, TriggeredJobWithTasksModel[]>
{
    private readonly IJobDb db;

    public TriggeredJobsAction(IJobDb db)
    {
        this.db = db;
    }

    public Task<TriggeredJobWithTasksModel[]> Execute(TriggeredJobsRequest model, CancellationToken ct) =>
        db.TriggeredJobs(model.EventNotificationID);
}
