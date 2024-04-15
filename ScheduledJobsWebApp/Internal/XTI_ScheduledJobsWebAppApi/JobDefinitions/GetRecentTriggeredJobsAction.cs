using Microsoft.EntityFrameworkCore;

namespace XTI_ScheduledJobsWebAppApi.JobDefinitions;

internal sealed class GetRecentTriggeredJobsAction : AppAction<GetRecentTriggeredJobsByDefinitionRequest, JobSummaryModel[]>
{
    private readonly JobDbContext db;

    public GetRecentTriggeredJobsAction(JobDbContext db)
    {
        this.db = db;
    }

    public Task<JobSummaryModel[]> Execute(GetRecentTriggeredJobsByDefinitionRequest model, CancellationToken stoppingToken) =>
        db.ExpandedTriggeredJobs.Retrieve()
            .Where(j => j.JobDefinitionID == model.JobDefinitionID)
            .OrderByDescending(j => j.TimeJobStarted)
            .Take(50)
            .Select(j => new JobSummaryModel(j))
            .ToArrayAsync();
}
