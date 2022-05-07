using XTI_Core;

namespace XTI_Jobs;

public sealed class EventMonitorFactory
{
    private readonly IStoredEvents storedEvents;
    private readonly IClock clock;

    public EventMonitorFactory(IStoredEvents storedEvents, IClock clock)
    {
        this.storedEvents = storedEvents;
        this.clock = clock;
    }

    public EventMonitorBuilder When(EventKey eventKey) => new EventMonitorBuilder(storedEvents, clock, eventKey);
}

public sealed class EventMonitorBuilder
{
    private readonly IStoredEvents storedEvents;
    private readonly IClock clock;
    private readonly EventKey eventKey;

    internal EventMonitorBuilder(IStoredEvents storedEvents, IClock clock, EventKey eventKey)
    {
        this.storedEvents = storedEvents;
        this.clock = clock;
        this.eventKey = eventKey;
    }

    public EventMonitor Trigger(JobKey jobKey) => new EventMonitor(storedEvents, clock, eventKey, jobKey);
}
