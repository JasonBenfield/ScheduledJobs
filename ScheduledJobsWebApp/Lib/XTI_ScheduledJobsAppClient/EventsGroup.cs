// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class EventsGroup : AppClientGroup
{
    public EventsGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl, AppClientOptions options) : base(httpClientFactory, xtiTokenAccessor, clientUrl, options, "Events")
    {
        Actions = new EventsGroupActions(AddNotifications: CreatePostAction<AddNotificationsRequest, EventNotificationModel[]>("AddNotifications"), AddOrUpdateRegisteredEvents: CreatePostAction<RegisteredEvent[], EmptyActionResult>("AddOrUpdateRegisteredEvents"), TriggeredJobs: CreatePostAction<TriggeredJobsRequest, TriggeredJobWithTasksModel[]>("TriggeredJobs"));
    }

    public EventsGroupActions Actions { get; }

    public Task<EventNotificationModel[]> AddNotifications(AddNotificationsRequest requestData, CancellationToken ct = default) => Actions.AddNotifications.Post("", requestData, ct);
    public Task<EmptyActionResult> AddOrUpdateRegisteredEvents(RegisteredEvent[] requestData, CancellationToken ct = default) => Actions.AddOrUpdateRegisteredEvents.Post("", requestData, ct);
    public Task<TriggeredJobWithTasksModel[]> TriggeredJobs(TriggeredJobsRequest requestData, CancellationToken ct = default) => Actions.TriggeredJobs.Post("", requestData, ct);
    public sealed record EventsGroupActions(AppClientPostAction<AddNotificationsRequest, EventNotificationModel[]> AddNotifications, AppClientPostAction<RegisteredEvent[], EmptyActionResult> AddOrUpdateRegisteredEvents, AppClientPostAction<TriggeredJobsRequest, TriggeredJobWithTasksModel[]> TriggeredJobs);
}