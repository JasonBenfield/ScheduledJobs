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
    public Task<ResultContainer<EventNotificationModel[]>> AddNotifications([FromBody] AddNotificationsRequest requestData, CancellationToken ct)
    {
        return api.Events.AddNotifications.Execute(requestData, ct);
    }

    [HttpPost]
    public Task<ResultContainer<EmptyActionResult>> AddOrUpdateRegisteredEvents([FromBody] RegisteredEvent[] requestData, CancellationToken ct)
    {
        return api.Events.AddOrUpdateRegisteredEvents.Execute(requestData, ct);
    }

    [HttpPost]
    public Task<ResultContainer<TriggeredJobWithTasksModel[]>> TriggeredJobs([FromBody] TriggeredJobsRequest requestData, CancellationToken ct)
    {
        return api.Events.TriggeredJobs.Execute(requestData, ct);
    }
}