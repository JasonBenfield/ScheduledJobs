using Microsoft.EntityFrameworkCore;

namespace XTI_ScheduledJobsWebAppApiActions.JobInquiry;

public sealed class GetFailedJobsAction : AppAction<EmptyRequest, JobSummaryModel[]>
{
    private readonly JobDbContext db;

    public GetFailedJobsAction(JobDbContext db)
    {
        this.db = db;
    }

    public Task<JobSummaryModel[]> Execute(EmptyRequest model, CancellationToken stoppingToken) =>
        db.ExpandedTriggeredJobs.Retrieve()
            .Where(j => j.JobStatus == JobTaskStatus.Values.Failed.Value)
            .OrderBy(j => j.TimeJobStarted)
            .Select(j => new JobSummaryModel(j))
            .ToArrayAsync();
}
