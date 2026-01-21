namespace XTI_Jobs;

public sealed class EventNotification
{
    private readonly IJobDb db;
    private readonly EventNotificationModel notification;

    internal EventNotification(IJobDb db, EventNotificationModel notification)
    {
        this.db = db;
        this.notification = notification;
    }

    public EventNotificationModel ToModel() => notification;

    public async Task<TriggeredJob[]> TriggeredJobs(CancellationToken ct)
    {
        var jobs = await db.TriggeredJobs(notification.ID, ct);
        return jobs.Select(j => new TriggeredJob(db, j)).ToArray();
    }
}
