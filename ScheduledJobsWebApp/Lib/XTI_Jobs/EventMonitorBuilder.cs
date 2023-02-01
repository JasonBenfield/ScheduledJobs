namespace XTI_Jobs;

public sealed class EventMonitorBuilder
{
    private readonly IJobDb db;
    private EventKey eventKey = new("");
    private JobKey jobKey = new("");
    private IJobActionFactory? jobActionFactory;
    private ITransformedEventData? transformedEventData;
    private DateTimeOffset eventRaisedStartTime = DateTimeOffset.MinValue;

    public EventMonitorBuilder(IJobDb db)
    {
        this.db = db;
    }

    public ScheduleMonitorBuilder1 WhenScheduled(JobKey jobKey)
    {
        this.jobKey = jobKey;
        eventKey = EventKey.Scheduled(jobKey);
        transformedEventData = new UnchangedEventData();
        return new ScheduleMonitorBuilder1(this);
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

public sealed class ScheduleMonitorBuilder1
{
    private readonly EventMonitorBuilder builder;

    public ScheduleMonitorBuilder1(EventMonitorBuilder builder)
    {
        this.builder = builder;
    }

    public EventMonitorBuilderFinal UseJobActionFactory(IJobActionFactory jobActionFactory)
    {
        builder.UseJobActionFactory(jobActionFactory);
        return new EventMonitorBuilderFinal(builder);
    }
}
