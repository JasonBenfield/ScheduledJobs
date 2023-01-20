namespace XTI_Jobs;

public sealed class EventMonitorBuilder
{
    private readonly IJobDb db;
    private EventKey eventKey = new EventKey("");
    private JobKey jobKey = new JobKey("");
    private IJobActionFactory? jobActionFactory;
    private ITransformedEventData? transformedEventData;
    private DateTimeOffset eventRaisedStartTime = DateTimeOffset.MinValue;

    public EventMonitorBuilder(IJobDb db)
    {
        this.db = db;
    }

    public EventMonitorBuilder1 When(EventKey eventKey)
    {
        this.eventKey = eventKey;
        return new EventMonitorBuilder1(this);
    }

    internal void HandleEventsRaisedOnOrAfter(DateTimeOffset eventRaisedStartTime)
    {
        this.eventRaisedStartTime = eventRaisedStartTime;
    }

    internal void Trigger(JobKey jobKey)
    {
        this.jobKey = jobKey;
    }

    internal void UseJobActionFactory(IJobActionFactory jobActionFactory)
    {
        this.jobActionFactory = jobActionFactory;
    }

    internal void TransformEventData(ITransformedEventData transformedEventData)
    {
        this.transformedEventData = transformedEventData;
    }

    internal EventMonitor Build() =>
        new EventMonitor
        (
            db,
            jobActionFactory ?? throw new ArgumentNullException(nameof(jobActionFactory)),
            transformedEventData ?? throw new ArgumentNullException(nameof(transformedEventData)),
            eventKey,
            jobKey,
            eventRaisedStartTime
        );
}
