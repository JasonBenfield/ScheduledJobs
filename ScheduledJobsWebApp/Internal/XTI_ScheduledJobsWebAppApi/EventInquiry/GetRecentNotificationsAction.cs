using Microsoft.EntityFrameworkCore;

namespace XTI_ScheduledJobsWebAppApi.EventInquiry;

internal sealed class GetRecentNotificationsAction : AppAction<EmptyRequest, EventSummaryModel[]>
{
    private readonly JobDbContext db;

    public GetRecentNotificationsAction(JobDbContext db)
    {
        this.db = db;
    }

    public async Task<EventSummaryModel[]> Execute(EmptyRequest model, CancellationToken stoppingToken)
    {
        var evtModels = await new EfEventNotificationInquiry(db).Recent();
        var evtWithDefs = await db.EventNotifications.Retrieve()
            .Join
            (
                db.EventDefinitions.Retrieve(),
                n => n.EventDefinitionID,
                d => d.ID,
                (n, d) => new { Notification = n, Definition = d }
            )
            .OrderByDescending(joined => joined.Notification.TimeAdded)
            .Take(50)
            .ToArrayAsync();
        var summaries = new List<EventSummaryModel>();
        foreach(var evtModel in evtModels)
        {
            var jobCount = await db.TriggeredJobs.Retrieve()
                .Where(j => j.EventNotificationID == evtModel.ID)
                .CountAsync();
            summaries.Add(new EventSummaryModel(evtModel, jobCount));
        }
        return summaries.ToArray();
    }
}
