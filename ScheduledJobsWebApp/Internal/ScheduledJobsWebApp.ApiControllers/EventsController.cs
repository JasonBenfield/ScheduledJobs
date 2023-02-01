// Generated Code
namespace ScheduledJobsWebApp.ApiControllers;
[Authorize]
public sealed partial class EventsController : Controller
{
    private readonly ScheduledJobsAppApi api;
    public EventsController(ScheduledJobsAppApi api)
    {
        this.api = api;
    }

    [HttpPost]
    public Task<ResultContainer<EmptyActionResult>> AddJobScheduleNotifications(CancellationToken ct)
    {
        return api.Group("Events").Action<EmptyRequest, EmptyActionResult>("AddJobScheduleNotifications").Execute(new EmptyRequest(), ct);
    }

    [HttpPost]
    public Task<ResultContainer<EmptyActionResult>> AddOrUpdateRegisteredEvents([FromBody] RegisteredEvent[] model, CancellationToken ct)
    {
        return api.Group("Events").Action<RegisteredEvent[], EmptyActionResult>("AddOrUpdateRegisteredEvents").Execute(model, ct);
    }

    [HttpPost]
    public Task<ResultContainer<EventNotificationModel[]>> AddNotifications([FromBody] AddNotificationsRequest model, CancellationToken ct)
    {
        return api.Group("Events").Action<AddNotificationsRequest, EventNotificationModel[]>("AddNotifications").Execute(model, ct);
    }

    [HttpPost]
    public Task<ResultContainer<TriggeredJobWithTasksModel[]>> TriggeredJobs([FromBody] TriggeredJobsRequest model, CancellationToken ct)
    {
        return api.Group("Events").Action<TriggeredJobsRequest, TriggeredJobWithTasksModel[]>("TriggeredJobs").Execute(model, ct);
    }
}