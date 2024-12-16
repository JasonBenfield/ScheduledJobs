using Microsoft.EntityFrameworkCore;

namespace XTI_ScheduledJobsWebAppApiActions.EventInquiry;

public sealed class GetNotificationDetailAction : AppAction<GetNotificationDetailRequest, EventNotificationDetailModel>
{
    private readonly JobDbContext db;

    public GetNotificationDetailAction(JobDbContext db)
    {
        this.db = db;
    }

    public async Task<EventNotificationDetailModel> Execute(GetNotificationDetailRequest model, CancellationToken stoppingToken)
    {
        var inquiry = new EfEventNotificationInquiry(db);
        var evt = await inquiry.Notification(model.NotificationID);
        var triggeredJobs = await db.ExpandedTriggeredJobs.Retrieve()
            .Where(j => j.EventNotificationID == model.NotificationID)
            .OrderBy(j => j.TimeJobStarted)
            .Select(j => new JobSummaryModel(j))
            .ToArrayAsync();
        return new EventNotificationDetailModel(evt, triggeredJobs);
    }
}
