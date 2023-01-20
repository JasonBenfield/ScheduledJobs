namespace XTI_Jobs;

public sealed class EventMonitorBuilder2
{
    private readonly EventMonitorBuilder builder;

    internal EventMonitorBuilder2(EventMonitorBuilder builder)
    {
        this.builder = builder;
    }

    public EventMonitorBuilder3 UseJobActionFactory(IJobActionFactory jobActionFactory)
    {
        builder.UseJobActionFactory(jobActionFactory);
        return new EventMonitorBuilder3(builder);
    }
}
