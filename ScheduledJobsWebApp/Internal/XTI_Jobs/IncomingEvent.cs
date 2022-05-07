using XTI_Core;

namespace XTI_Jobs;

public sealed class IncomingEvent
{
    private readonly IStoredEvents storedEvents;
    private readonly IClock clock;
    private readonly EventKey eventKey;
    private readonly EventSource[] sources;

    internal IncomingEvent(IStoredEvents storedEvents, IClock clock, EventKey eventKey, EventSource[] sources)
    {
        this.storedEvents = storedEvents;
        this.clock = clock;
        this.eventKey = eventKey;
        this.sources = sources;
    }

    public async Task<EventNotification[]> Notify()
    {
        var notificationModels = await storedEvents.AddNotifications(eventKey, sources, clock.Now());
        return notificationModels.Select(n => new EventNotification(storedEvents, n)).ToArray();
    }
}
