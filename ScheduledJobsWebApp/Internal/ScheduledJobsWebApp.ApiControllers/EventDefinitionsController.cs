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

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var result = await api.Group("EventDefinitions").Action<EmptyRequest, WebViewResult>("Index").Execute(new EmptyRequest(), ct);
        return View(result.Data!.ViewName);
    }

    [HttpPost]
    public Task<ResultContainer<EventDefinitionModel[]>> GetEventDefinitions(CancellationToken ct)
    {
        return api.Group("EventDefinitions").Action<EmptyRequest, EventDefinitionModel[]>("GetEventDefinitions").Execute(new EmptyRequest(), ct);
    }

    [HttpPost]
    public Task<ResultContainer<EventSummaryModel[]>> GetRecentNotifications([FromBody] GetRecentEventNotificationsByEventDefinitionRequest model, CancellationToken ct)
    {
        return api.Group("EventDefinitions").Action<GetRecentEventNotificationsByEventDefinitionRequest, EventSummaryModel[]>("GetRecentNotifications").Execute(model, ct);
    }
}