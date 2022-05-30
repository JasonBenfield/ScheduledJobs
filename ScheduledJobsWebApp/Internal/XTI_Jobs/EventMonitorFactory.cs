using XTI_Core;

namespace XTI_Jobs;

public sealed class EventMonitorFactory
{
    private readonly IJobDb storedEvents;
    private readonly IJobActionFactory jobActionFactory;

    public EventMonitorFactory(IJobDb storedEvents, IJobActionFactory jobActionFactory)
    {
        this.storedEvents = storedEvents;
        this.jobActionFactory = jobActionFactory;
    }

    public EventMonitorBuilder When(EventKey eventKey) => new EventMonitorBuilder(storedEvents, jobActionFactory, eventKey);
}

public sealed class EventMonitorBuilder
{
    private readonly IJobDb storedEvents;
    private readonly IJobActionFactory jobActionFactory;
    private readonly EventKey eventKey;

    internal EventMonitorBuilder(IJobDb storedEvents, IJobActionFactory jobActionFactory, EventKey eventKey)
    {
        this.storedEvents = storedEvents;
        this.jobActionFactory = jobActionFactory;
        this.eventKey = eventKey;
    }

    public EventMonitor Trigger(JobKey jobKey) => new EventMonitor(storedEvents, jobActionFactory, eventKey, jobKey);
}
