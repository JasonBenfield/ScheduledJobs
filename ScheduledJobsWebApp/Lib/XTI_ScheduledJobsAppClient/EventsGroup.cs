// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class EventsGroup : AppClientGroup
{
    public EventsGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl, AppClientOptions options) : base(httpClientFactory, xtiTokenAccessor, clientUrl, options, "Events")
    {
        Actions = new EventsGroupActions(AddJobScheduleNotifications: CreatePostAction<EmptyRequest, EmptyActionResult>("AddJobScheduleNotifications"), AddOrUpdateRegisteredEvents: CreatePostAction<RegisteredEvent[], EmptyActionResult>("AddOrUpdateRegisteredEvents"), AddNotifications: CreatePostAction<AddNotificationsRequest, EventNotificationModel[]>("AddNotifications"), TriggeredJobs: CreatePostAction<TriggeredJobsRequest, TriggeredJobWithTasksModel[]>("TriggeredJobs"));
    }

    public EventsGroupActions Actions { get; }

    public Task<EmptyActionResult> AddJobScheduleNotifications(CancellationToken ct = default) => Actions.AddJobScheduleNotifications.Post("", new EmptyRequest(), ct);
    public Task<EmptyActionResult> AddOrUpdateRegisteredEvents(RegisteredEvent[] model, CancellationToken ct = default) => Actions.AddOrUpdateRegisteredEvents.Post("", model, ct);
    public Task<EventNotificationModel[]> AddNotifications(AddNotificationsRequest model, CancellationToken ct = default) => Actions.AddNotifications.Post("", model, ct);
    public Task<TriggeredJobWithTasksModel[]> TriggeredJobs(TriggeredJobsRequest model, CancellationToken ct = default) => Actions.TriggeredJobs.Post("", model, ct);
    public sealed record EventsGroupActions(AppClientPostAction<EmptyRequest, EmptyActionResult> AddJobScheduleNotifications, AppClientPostAction<RegisteredEvent[], EmptyActionResult> AddOrUpdateRegisteredEvents, AppClientPostAction<AddNotificationsRequest, EventNotificationModel[]> AddNotifications, AppClientPostAction<TriggeredJobsRequest, TriggeredJobWithTasksModel[]> TriggeredJobs);
}