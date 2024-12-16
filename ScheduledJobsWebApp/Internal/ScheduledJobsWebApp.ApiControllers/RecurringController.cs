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
        return api.Recurring.AddJobScheduleNotifications.Execute(new EmptyRequest(), ct);
    }

    [HttpPost]
    public Task<ResultContainer<EmptyActionResult>> PurgeJobsAndEvents(CancellationToken ct)
    {
        return api.Recurring.PurgeJobsAndEvents.Execute(new EmptyRequest(), ct);
    }

    [HttpPost]
    public Task<ResultContainer<EmptyActionResult>> TimeoutTasks(CancellationToken ct)
    {
        return api.Recurring.TimeoutTasks.Execute(new EmptyRequest(), ct);
    }
}