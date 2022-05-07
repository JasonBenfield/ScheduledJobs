using XTI_Core;

namespace XTI_Jobs;

public sealed class EventNotification
{
    private readonly IStoredEvents storedEvents;
    private readonly EventNotificationModel notification;

    internal EventNotification(IStoredEvents storedEvents, EventNotificationModel notification)
    {
        this.storedEvents = storedEvents;
        this.notification = notification;
    }

    public Task<TriggeredJobModel[]> TriggeredJobs() => storedEvents.TriggeredJobs(notification);
}
