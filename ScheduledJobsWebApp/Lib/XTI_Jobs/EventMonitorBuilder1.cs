namespace XTI_Jobs;

public sealed class EventMonitorBuilder1
{
    private readonly EventMonitorBuilder builder;

    internal EventMonitorBuilder1(EventMonitorBuilder builder)
    {
        this.builder = builder;
    }

    public EventMonitorBuilder1 HandleEventsRaisedOnOrAfter(DateTimeOffset eventRaisedStartTime)
    {
        builder.HandleEventsRaisedOnOrAfter(eventRaisedStartTime);
        return this;
    }

    public EventMonitorBuilder2 Trigger(JobKey jobKey)
    {
        builder.Trigger(jobKey);
        return new EventMonitorBuilder2(builder);
    }
}
