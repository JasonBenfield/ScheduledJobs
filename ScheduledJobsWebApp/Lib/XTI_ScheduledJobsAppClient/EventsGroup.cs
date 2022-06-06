// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class EventsGroup : AppClientGroup
{
    public EventsGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl) : base(httpClientFactory, xtiTokenAccessor, clientUrl, "Events")
    {
    }

    public Task<EmptyActionResult> AddOrUpdateRegisteredEvents(RegisteredEvent[] model) => Post<EmptyActionResult, RegisteredEvent[]>("AddOrUpdateRegisteredEvents", "", model);
    public Task<EventNotificationModel[]> AddNotifications(AddNotificationsRequest model) => Post<EventNotificationModel[], AddNotificationsRequest>("AddNotifications", "", model);
    public Task<TriggeredJobWithTasksModel[]> TriggeredJobs(TriggeredJobsRequest model) => Post<TriggeredJobWithTasksModel[], TriggeredJobsRequest>("TriggeredJobs", "", model);
}