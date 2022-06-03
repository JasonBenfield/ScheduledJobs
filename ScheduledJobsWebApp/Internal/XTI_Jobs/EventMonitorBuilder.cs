namespace XTI_Jobs;

public sealed class EventMonitorBuilder
{
    private readonly IJobDb db;
    private readonly IJobActionFactory jobActionFactory;
    private readonly EventKey eventKey;
    private DateTimeOffset eventRaisedStartTime = DateTimeOffset.MinValue;

    internal EventMonitorBuilder(IJobDb db, IJobActionFactory jobActionFactory, EventKey eventKey)
    {
        this.db = db;
        this.jobActionFactory = jobActionFactory;
        this.eventKey = eventKey;
    }

    public EventMonitorBuilder HandleEventsRaisedOnOrAfter(DateTimeOffset eventRaisedStartTime)
    {
        this.eventRaisedStartTime = eventRaisedStartTime;
        return this;
    }

    public EventMonitor Trigger(JobKey jobKey) => 
        new EventMonitor(db, jobActionFactory, eventKey, jobKey, eventRaisedStartTime);
}
