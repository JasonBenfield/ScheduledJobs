namespace XTI_ScheduledJobsWebAppApi.Events;

public sealed class EventsGroup : AppApiGroupWrapper
{
    public EventsGroup(AppApiGroup source, IServiceProvider sp)
        : base(source)
    {
        var actions = new AppApiActionFactory(source);
        AddOrUpdateRegisteredEvents = source.AddAction
        (
            actions.Action
            (
                nameof(AddOrUpdateRegisteredEvents),
                () => sp.GetRequiredService<AddOrUpdateRegisteredEventsAction>()
            )
        );
        AddNotifications = source.AddAction
        (
            actions.Action(nameof(AddNotifications), () => sp.GetRequiredService<AddNotificationsAction>())
        );
        TriggeredJobs = source.AddAction
        (
            actions.Action(nameof(TriggeredJobs), () => sp.GetRequiredService<TriggeredJobsAction>())
        );
    }

    public AppApiAction<RegisteredEvent[], EmptyActionResult> AddOrUpdateRegisteredEvents { get; }
    public AppApiAction<AddNotificationsRequest, EventNotificationModel[]> AddNotifications { get; }
    public AppApiAction<TriggeredJobsRequest, TriggeredJobWithTasksModel[]> TriggeredJobs { get; }
}