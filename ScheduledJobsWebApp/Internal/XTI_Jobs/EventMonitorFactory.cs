namespace XTI_Jobs;

public sealed class EventMonitorFactory
{
    private readonly IJobDb db;
    private readonly IJobActionFactory jobActionFactory;

    public EventMonitorFactory(IJobDb db, IJobActionFactory jobActionFactory)
    {
        this.db = db;
        this.jobActionFactory = jobActionFactory;
    }

    public EventMonitorBuilder When(EventKey eventKey) => new EventMonitorBuilder(db, jobActionFactory, eventKey);
}
