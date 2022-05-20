using XTI_Core;

namespace XTI_Jobs;

public sealed class EventNotification
{
    private readonly IJobDb db;
    private readonly IClock clock;
    private readonly EventNotificationModel notification;

    internal EventNotification(IJobDb db, IClock clock, EventNotificationModel notification)
    {
        this.db = db;
        this.clock = clock;
        this.notification = notification;
    }

    public async Task<TriggeredJob[]> TriggeredJobs()
    {
        var jobs = await db.TriggeredJobs(notification);
        return jobs.Select(j => new TriggeredJob(db, clock, j)).ToArray();
    }
}
