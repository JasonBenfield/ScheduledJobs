namespace XTI_Jobs;

public sealed class EventMonitorBuilder3
{
    private readonly EventMonitorBuilder builder;

    internal EventMonitorBuilder3(EventMonitorBuilder builder)
    {
        this.builder = builder;
    }

    public EventMonitorBuilderFinal TransformEventData(ITransformedEventData transformedEventData)
    {
        builder.TransformEventData(transformedEventData);
        return new EventMonitorBuilderFinal(builder);
    }
}
