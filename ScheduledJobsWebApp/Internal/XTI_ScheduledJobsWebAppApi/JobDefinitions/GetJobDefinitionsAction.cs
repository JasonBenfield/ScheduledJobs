using Microsoft.EntityFrameworkCore;

namespace XTI_ScheduledJobsWebAppApi.JobDefinitions;

internal sealed class GetJobDefinitionsAction : AppAction<EmptyRequest, JobDefinitionModel[]>
{
    private readonly JobDbContext db;

    public GetJobDefinitionsAction(JobDbContext db)
    {
        this.db = db;
    }

    public Task<JobDefinitionModel[]> Execute(EmptyRequest model, CancellationToken stoppingToken) =>
        db.JobDefinitions.Retrieve()
            .OrderBy(jobDef => jobDef.DisplayText)
            .Select(jobDef => new JobDefinitionModel(jobDef.ID, new JobKey(jobDef.DisplayText)))
            .ToArrayAsync();
}
