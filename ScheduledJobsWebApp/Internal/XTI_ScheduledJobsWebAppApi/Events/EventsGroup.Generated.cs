using XTI_ScheduledJobsWebAppApiActions.Events;

// Generated Code
#nullable enable
namespace XTI_ScheduledJobsWebAppApi.Events;
public sealed partial class EventsGroup : AppApiGroupWrapper
{
    internal EventsGroup(AppApiGroup source, EventsGroupBuilder builder) : base(source)
    {
        AddNotifications = builder.AddNotifications.Build();
        AddOrUpdateRegisteredEvents = builder.AddOrUpdateRegisteredEvents.Build();
        TriggeredJobs = builder.TriggeredJobs.Build();
        Configure();
    }

    partial void Configure();
    public AppApiAction<AddNotificationsRequest, EventNotificationModel[]> AddNotifications { get; }
    public AppApiAction<RegisteredEvent[], EmptyActionResult> AddOrUpdateRegisteredEvents { get; }
    public AppApiAction<TriggeredJobsRequest, TriggeredJobWithTasksModel[]> TriggeredJobs { get; }
}