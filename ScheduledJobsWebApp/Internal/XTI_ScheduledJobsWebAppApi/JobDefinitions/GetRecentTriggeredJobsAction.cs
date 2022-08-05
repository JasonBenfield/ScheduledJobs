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
            .OrderBy(j => j.TimeJobStarted)
            .Select(j => new JobSummaryModel(j))
            .ToArrayAsync();
}
