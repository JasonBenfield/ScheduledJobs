using XTI_ScheduledJobsWebAppApiActions.Events;

// Generated Code
#nullable enable
namespace XTI_ScheduledJobsWebAppApi.Events;
public sealed partial class EventsGroupBuilder
{
    private readonly AppApiGroup source;
    internal EventsGroupBuilder(AppApiGroup source)
    {
        this.source = source;
        AddNotifications = source.AddAction<AddNotificationsRequest, EventNotificationModel[]>("AddNotifications").WithExecution<AddNotificationsAction>();
        AddOrUpdateRegisteredEvents = source.AddAction<RegisteredEvent[], EmptyActionResult>("AddOrUpdateRegisteredEvents").WithExecution<AddOrUpdateRegisteredEventsAction>();
        TriggeredJobs = source.AddAction<TriggeredJobsRequest, TriggeredJobWithTasksModel[]>("TriggeredJobs").WithExecution<TriggeredJobsAction>();
        Configure();
    }

    partial void Configure();
    public AppApiActionBuilder<AddNotificationsRequest, EventNotificationModel[]> AddNotifications { get; }
    public AppApiActionBuilder<RegisteredEvent[], EmptyActionResult> AddOrUpdateRegisteredEvents { get; }
    public AppApiActionBuilder<TriggeredJobsRequest, TriggeredJobWithTasksModel[]> TriggeredJobs { get; }

    public EventsGroup Build() => new EventsGroup(source, this);
}