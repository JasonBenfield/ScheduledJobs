namespace XTI_ScheduledJobsWebAppApi.JobInquiry;

internal sealed class GetJobDetailAction : AppAction<GetJobDetailRequest, TriggeredJobDetailModel>
{
    private readonly JobDbContext db;

    public GetJobDetailAction(JobDbContext db)
    {
        this.db = db;
    }

    public Task<TriggeredJobDetailModel> Execute(GetJobDetailRequest model, CancellationToken stoppingToken) =>
        new EfTriggeredJobDetail(db, model.JobID).Value();
}
