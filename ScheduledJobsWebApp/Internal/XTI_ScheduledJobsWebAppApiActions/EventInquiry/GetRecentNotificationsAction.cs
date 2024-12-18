namespace XTI_ScheduledJobsWebAppApiActions.EventInquiry;

public sealed class GetRecentNotificationsAction : AppAction<EmptyRequest, EventSummaryModel[]>
{
    private readonly JobDbContext db;

    public GetRecentNotificationsAction(JobDbContext db)
    {
        this.db = db;
    }

    public async Task<EventSummaryModel[]> Execute(EmptyRequest model, CancellationToken stoppingToken)
    {
        var inquiry = new EfEventNotificationInquiry(db);
        var evtModels = await inquiry.Recent();
        var summaries = new List<EventSummaryModel>();
        foreach(var evtModel in evtModels)
        {
            var jobCount = await inquiry.JobCount(evtModel.ID);
            summaries.Add(new EventSummaryModel(evtModel, jobCount));
        }
        return summaries.ToArray();
    }
}
