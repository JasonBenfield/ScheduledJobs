namespace XTI_Jobs.Abstractions;

public interface IStoredEvents
{
    Task AddOrUpdateRegisteredEvents(RegisteredEvent[] registeredEvents);

    Task AddOrUpdateRegisteredJobs(RegisteredJob[] registeredJobs);

    Task<EventNotificationModel[]> AddNotifications(EventKey eventKey, EventSource[] sources, DateTimeOffset now);

    Task<TriggeredJobModel[]> TriggerJobs(EventKey eventKey, JobKey jobKey, DateTimeOffset now);

    Task<TriggeredJobModel[]> TriggeredJobs(EventNotificationModel notification);
}