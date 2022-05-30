// Generated Code
namespace ScheduledJobsWebApp.ApiControllers;
[Authorize]
public class EventsController : Controller
{
    private readonly ScheduledJobsAppApi api;
    public EventsController(ScheduledJobsAppApi api)
    {
        this.api = api;
    }

    [HttpPost]
    public Task<ResultContainer<EmptyActionResult>> AddOrUpdateRegisteredEvents([FromBody] RegisteredEvent[] model)
    {
        return api.Group("Events").Action<RegisteredEvent[], EmptyActionResult>("AddOrUpdateRegisteredEvents").Execute(model);
    }

    [HttpPost]
    public Task<ResultContainer<EventNotificationModel[]>> AddNotifications([FromBody] AddNotificationsRequest model)
    {
        return api.Group("Events").Action<AddNotificationsRequest, EventNotificationModel[]>("AddNotifications").Execute(model);
    }

    [HttpPost]
    public Task<ResultContainer<TriggeredJobDetailModel[]>> TriggeredJobs([FromBody] TriggeredJobsRequest model)
    {
        return api.Group("Events").Action<TriggeredJobsRequest, TriggeredJobDetailModel[]>("TriggeredJobs").Execute(model);
    }
}