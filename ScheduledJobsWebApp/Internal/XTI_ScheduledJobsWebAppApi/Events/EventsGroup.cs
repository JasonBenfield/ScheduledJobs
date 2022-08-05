namespace XTI_ScheduledJobsWebAppApi.Events;

public sealed class EventsGroup : AppApiGroupWrapper
{
    public EventsGroup(AppApiGroup source, IServiceProvider sp)
        : base(source)
    {
        AddOrUpdateRegisteredEvents = source.AddAction
        (
            nameof(AddOrUpdateRegisteredEvents),
            () => sp.GetRequiredService<AddOrUpdateRegisteredEventsAction>()
        );
        AddNotifications = source.AddAction
        (
            nameof(AddNotifications), () => sp.GetRequiredService<AddNotificationsAction>()
        );
        TriggeredJobs = source.AddAction
        (
            nameof(TriggeredJobs), () => sp.GetRequiredService<TriggeredJobsAction>()
        );
    }

    public AppApiAction<RegisteredEvent[], EmptyActionResult> AddOrUpdateRegisteredEvents { get; }
    public AppApiAction<AddNotificationsRequest, EventNotificationModel[]> AddNotifications { get; }
    public AppApiAction<TriggeredJobsRequest, TriggeredJobWithTasksModel[]> TriggeredJobs { get; }
}