namespace XTI_Jobs;

public sealed class EventMonitorBuilder2
{
    private readonly EventMonitorBuilder builder;

    internal EventMonitorBuilder2(EventMonitorBuilder builder)
    {
        this.builder = builder;
    }

    public EventMonitor UseJobActionFactory(IJobActionFactory jobActionFactory)
    {
        builder.UseJobActionFactory(jobActionFactory);
        return builder.Build();
    }
}
