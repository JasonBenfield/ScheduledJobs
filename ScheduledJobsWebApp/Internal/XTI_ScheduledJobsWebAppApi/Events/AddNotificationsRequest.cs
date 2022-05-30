namespace XTI_ScheduledJobsWebAppApi.Events;

public sealed record AddNotificationsRequest
(
    EventKey EventKey, 
    EventSource[] Sources
);
