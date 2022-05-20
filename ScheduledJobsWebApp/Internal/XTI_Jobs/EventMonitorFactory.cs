using XTI_Core;

namespace XTI_Jobs;

public sealed class EventMonitorFactory
{
    private readonly IJobDb storedEvents;
    private readonly IJobActionFactory jobActionFactory;
    private readonly IClock clock;

    public EventMonitorFactory(IJobDb storedEvents, IJobActionFactory jobActionFactory, IClock clock)
    {
        this.storedEvents = storedEvents;
        this.jobActionFactory = jobActionFactory;
        this.clock = clock;
    }

    public EventMonitorBuilder When(EventKey eventKey) => new EventMonitorBuilder(storedEvents, jobActionFactory, clock, eventKey);
}

public sealed class EventMonitorBuilder
{
    private readonly IJobDb storedEvents;
    private readonly IJobActionFactory jobActionFactory;
    private readonly IClock clock;
    private readonly EventKey eventKey;

    internal EventMonitorBuilder(IJobDb storedEvents, IJobActionFactory jobActionFactory, IClock clock, EventKey eventKey)
    {
        this.storedEvents = storedEvents;
        this.jobActionFactory = jobActionFactory;
        this.clock = clock;
        this.eventKey = eventKey;
    }

    public EventMonitor Trigger(JobKey jobKey) => new EventMonitor(storedEvents, jobActionFactory, clock, eventKey, jobKey);
}
