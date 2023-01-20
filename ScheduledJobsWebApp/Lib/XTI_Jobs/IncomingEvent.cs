using XTI_Core;

namespace XTI_Jobs;

public sealed class IncomingEvent
{
    private readonly IJobDb db;
    private readonly IClock clock;
    private readonly EventKey eventKey;
    private readonly XtiEventSource[] sources;

    internal IncomingEvent(IJobDb db, IClock clock, EventKey eventKey, XtiEventSource[] sources)
    {
        this.db = db;
        this.clock = clock;
        this.eventKey = eventKey;
        this.sources = sources;
    }

    public async Task<EventNotification[]> Notify()
    {
        var notificationModels = await db.AddEventNotifications(eventKey, sources);
        return notificationModels.Select(n => new EventNotification(db, n)).ToArray();
    }
}
