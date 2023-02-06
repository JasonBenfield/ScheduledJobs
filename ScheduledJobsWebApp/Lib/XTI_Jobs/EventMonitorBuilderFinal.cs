namespace XTI_Jobs;

public sealed class EventMonitorBuilderFinal
{
    private readonly EventMonitorBuilder builder;

    internal EventMonitorBuilderFinal(EventMonitorBuilder builder)
    {
        this.builder = builder;
    }

    public EventMonitorBuilderFinal HandleEventsRaisedOnOrAfter(DateTimeOffset eventRaisedStartTime)
    {
        builder.HandleEventsRaisedOnOrAfter(eventRaisedStartTime);
        return this;
    }

    public EventMonitor Build() => builder.Build();
}
