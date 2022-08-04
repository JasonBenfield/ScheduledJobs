// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class EventsGroup : AppClientGroup
{
    public EventsGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl, AppClientOptions options) : base(httpClientFactory, xtiTokenAccessor, clientUrl, options, "Events")
    {
        Actions = new EventsGroupActions(AddOrUpdateRegisteredEvents: CreatePostAction<RegisteredEvent[], EmptyActionResult>("AddOrUpdateRegisteredEvents"), AddNotifications: CreatePostAction<AddNotificationsRequest, EventNotificationModel[]>("AddNotifications"), TriggeredJobs: CreatePostAction<TriggeredJobsRequest, TriggeredJobWithTasksModel[]>("TriggeredJobs"));
    }

    public EventsGroupActions Actions { get; }

    public Task<EmptyActionResult> AddOrUpdateRegisteredEvents(RegisteredEvent[] model) => Actions.AddOrUpdateRegisteredEvents.Post("", model);
    public Task<EventNotificationModel[]> AddNotifications(AddNotificationsRequest model) => Actions.AddNotifications.Post("", model);
    public Task<TriggeredJobWithTasksModel[]> TriggeredJobs(TriggeredJobsRequest model) => Actions.TriggeredJobs.Post("", model);
    public sealed record EventsGroupActions(AppClientPostAction<RegisteredEvent[], EmptyActionResult> AddOrUpdateRegisteredEvents, AppClientPostAction<AddNotificationsRequest, EventNotificationModel[]> AddNotifications, AppClientPostAction<TriggeredJobsRequest, TriggeredJobWithTasksModel[]> TriggeredJobs);
}