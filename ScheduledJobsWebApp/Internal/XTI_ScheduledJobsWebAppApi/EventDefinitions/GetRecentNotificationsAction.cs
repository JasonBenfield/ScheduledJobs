namespace XTI_ScheduledJobsWebAppApi.EventDefinitions;

internal sealed class GetRecentNotificationsAction : AppAction<GetRecentEventNotificationsByEventDefinitionRequest, EventSummaryModel[]>
{
    private readonly JobDbContext db;

    public GetRecentNotificationsAction(JobDbContext db)
    {
        this.db = db;
    }

    public async Task<EventSummaryModel[]> Execute(GetRecentEventNotificationsByEventDefinitionRequest model, CancellationToken stoppingToken)
    {
        var inquiry = new EfEventNotificationInquiry(db);
        var evtModels = await inquiry.Recent(model.EventDefinitionID, model.SourceKey);
        var summaries = new List<EventSummaryModel>();
        foreach (var evtModel in evtModels)
        {
            var jobCount = await inquiry.JobCount(evtModel.ID);
            summaries.Add(new EventSummaryModel(evtModel, jobCount));
        }
        return summaries.ToArray();
    }
}
