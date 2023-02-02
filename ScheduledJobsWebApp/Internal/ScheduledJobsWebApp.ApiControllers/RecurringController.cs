// Generated Code
namespace ScheduledJobsWebApp.ApiControllers;
[Authorize]
public sealed partial class RecurringController : Controller
{
    private readonly ScheduledJobsAppApi api;
    public RecurringController(ScheduledJobsAppApi api)
    {
        this.api = api;
    }

    [HttpPost]
    public Task<ResultContainer<EmptyActionResult>> AddJobScheduleNotifications(CancellationToken ct)
    {
        return api.Group("Recurring").Action<EmptyRequest, EmptyActionResult>("AddJobScheduleNotifications").Execute(new EmptyRequest(), ct);
    }

    [HttpPost]
    public Task<ResultContainer<EmptyActionResult>> TimeoutTasks(CancellationToken ct)
    {
        return api.Group("Recurring").Action<EmptyRequest, EmptyActionResult>("TimeoutTasks").Execute(new EmptyRequest(), ct);
    }

    [HttpPost]
    public Task<ResultContainer<EmptyActionResult>> PurgeJobsAndEvents(CancellationToken ct)
    {
        return api.Group("Recurring").Action<EmptyRequest, EmptyActionResult>("PurgeJobsAndEvents").Execute(new EmptyRequest(), ct);
    }
}