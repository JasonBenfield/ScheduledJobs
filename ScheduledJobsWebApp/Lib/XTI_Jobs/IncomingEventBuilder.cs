using XTI_Core;

namespace XTI_Jobs;

public sealed class IncomingEventBuilder
{
    private readonly IJobDb storedEvents;
    private readonly IClock clock;
    private readonly EventKey eventKey;

    internal IncomingEventBuilder(IJobDb storedEvents, IClock clock, EventKey eventKey)
    {
        this.storedEvents = storedEvents;
        this.clock = clock;
        this.eventKey = eventKey;
    }

    public IncomingEvent From(EventSource source) => new IncomingEvent(storedEvents, clock, eventKey, new[] { source });
}
