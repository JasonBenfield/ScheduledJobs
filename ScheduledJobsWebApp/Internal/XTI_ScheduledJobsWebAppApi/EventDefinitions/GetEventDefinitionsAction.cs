using Microsoft.EntityFrameworkCore;

namespace XTI_ScheduledJobsWebAppApi.EventDefinitions;

internal sealed class GetEventDefinitionsAction : AppAction<EmptyRequest, EventDefinitionModel[]>
{
    private readonly JobDbContext db;

    public GetEventDefinitionsAction(JobDbContext db)
    {
        this.db = db;
    }

    public Task<EventDefinitionModel[]> Execute(EmptyRequest model, CancellationToken stoppingToken) =>
        db.EventDefinitions.Retrieve()
            .OrderBy(ed => ed.DisplayText)
            .Select(ed => new EventDefinitionModel(ed.ID, new EventKey(ed.DisplayText)))
            .ToArrayAsync();
}
