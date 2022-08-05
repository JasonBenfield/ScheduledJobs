namespace XTI_ScheduledJobsWebAppApi.JobInquiry;

public sealed record TriggeredJobDetailModel
(
    TriggeredJobModel Job,
    EventNotificationModel TriggeredBy,
    TriggeredJobTaskModel[] Tasks
);

internal sealed class GetJobDetailAction : AppAction<GetJobDetailRequest, TriggeredJobDetailModel>
{
    private readonly JobDbContext db;

    public GetJobDetailAction(JobDbContext db)
    {
        this.db = db;
    }

    public async Task<TriggeredJobDetailModel> Execute(GetJobDetailRequest model, CancellationToken stoppingToken)
    {
        var jobWithTasks = await new EfTriggeredJobDetail(db, model.JobID).Value();
        var triggeredBy = await new EfEventNotificationInquiry(db).Notification(jobWithTasks.Job.EventNotificationID);
        return new TriggeredJobDetailModel(jobWithTasks.Job, triggeredBy, jobWithTasks.Tasks);
    }
}
