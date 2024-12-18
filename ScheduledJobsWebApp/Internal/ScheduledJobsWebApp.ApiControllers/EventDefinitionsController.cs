// Generated Code
namespace ScheduledJobsWebApp.ApiControllers;
[Authorize]
public sealed partial class EventDefinitionsController : Controller
{
    private readonly ScheduledJobsAppApi api;
    public EventDefinitionsController(ScheduledJobsAppApi api)
    {
        this.api = api;
    }

    [HttpPost]
    public Task<ResultContainer<EventDefinitionModel[]>> GetEventDefinitions(CancellationToken ct)
    {
        return api.EventDefinitions.GetEventDefinitions.Execute(new EmptyRequest(), ct);
    }

    [HttpPost]
    public Task<ResultContainer<EventSummaryModel[]>> GetRecentNotifications([FromBody] GetRecentEventNotificationsByEventDefinitionRequest requestData, CancellationToken ct)
    {
        return api.EventDefinitions.GetRecentNotifications.Execute(requestData, ct);
    }

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var result = await api.EventDefinitions.Index.Execute(new EmptyRequest(), ct);
        return View(result.Data!.ViewName);
    }
}