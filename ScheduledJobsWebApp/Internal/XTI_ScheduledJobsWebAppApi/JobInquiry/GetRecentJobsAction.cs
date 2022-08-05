using Microsoft.EntityFrameworkCore;

namespace XTI_ScheduledJobsWebAppApi.JobInquiry;

internal sealed class GetRecentJobsAction : AppAction<EmptyRequest, JobSummaryModel[]>
{
    private readonly JobDbContext db;

    public GetRecentJobsAction(JobDbContext db)
    {
        this.db = db;
    }

    public Task<JobSummaryModel[]> Execute(EmptyRequest model, CancellationToken stoppingToken) =>
        db.ExpandedTriggeredJobs.Retrieve()
            .OrderBy(j => j.TimeJobStarted)
            .Select(j => new JobSummaryModel(j))
            .ToArrayAsync();
}
