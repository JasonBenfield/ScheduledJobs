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
            .OrderByDescending(j => j.TimeJobStarted)
            .Take(50)
            .Select(j => new JobSummaryModel(j))
            .ToArrayAsync();
}
