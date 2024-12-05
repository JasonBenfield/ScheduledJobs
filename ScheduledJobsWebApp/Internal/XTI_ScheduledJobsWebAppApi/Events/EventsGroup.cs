using XTI_ScheduledJobsWebAppApi.Recurring;

namespace XTI_ScheduledJobsWebAppApi.Events;

public sealed class EventsGroup : AppApiGroupWrapper
{
    public EventsGroup(AppApiGroup source, IServiceProvider sp)
        : base(source)
    {
        AddOrUpdateRegisteredEvents = source.AddAction<RegisteredEvent[], EmptyActionResult>()
            .Named(nameof(AddOrUpdateRegisteredEvents))
            .WithExecution<AddOrUpdateRegisteredEventsAction>()
            .Build();
        AddNotifications = source.AddAction<AddNotificationsRequest, EventNotificationModel[]>()
            .Named(nameof(AddNotifications))
            .WithExecution<AddNotificationsAction>()
            .ThrottleRequestLogging().ForOneHour()
            .ThrottleExceptionLogging().For(5).Minutes()
            .Build();
        TriggeredJobs = source.AddAction<TriggeredJobsRequest, TriggeredJobWithTasksModel[]>()
            .Named(nameof(TriggeredJobs))
            .WithExecution<TriggeredJobsAction>()
            .Build();
    }

    public AppApiAction<RegisteredEvent[], EmptyActionResult> AddOrUpdateRegisteredEvents { get; }
    public AppApiAction<AddNotificationsRequest, EventNotificationModel[]> AddNotifications { get; }
    public AppApiAction<TriggeredJobsRequest, TriggeredJobWithTasksModel[]> TriggeredJobs { get; }
}