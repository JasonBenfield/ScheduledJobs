using XTI_Core;

namespace XTI_Jobs;

public sealed class IncomingEventFactory
{
    private readonly IStoredEvents storedEvents;
    private readonly IClock clock;

    public IncomingEventFactory(IStoredEvents storedEvents, IClock clock)
    {
        this.storedEvents = storedEvents;
        this.clock = clock;
    }

    public IncomingEventBuilder Incoming(EventKey eventKey) => new IncomingEventBuilder(storedEvents, clock, eventKey);
}