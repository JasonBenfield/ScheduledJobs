using XTI_Schedule;

namespace XTI_Jobs.Abstractions;

public interface IJobDb
{
    Task AddOrUpdateJobSchedules(JobKey jobKey, AggregateSchedule schedule, TimeSpan deleteAfter, CancellationToken ct);

    Task AddOrUpdateRegisteredEvents(RegisteredEvent[] registeredEvents, CancellationToken ct);

    Task AddOrUpdateRegisteredJobs(RegisteredJob[] registeredJobs, CancellationToken ct);

    Task<EventNotificationModel[]> AddEventNotifications(EventKey eventKey, XtiEventSource[] sources, CancellationToken ct);

    Task<TriggeredJobWithTasksModel[]> TriggeredJobs(int notificationID, CancellationToken ct);

    Task<PendingJobModel[]> TriggerJobs(EventKey eventKey, JobKey jobKey, DateTimeOffset eventRaisedStartTime, CancellationToken ct);

    Task DeleteJobsWithNoTasks(EventKey eventKey, JobKey jobKey, CancellationToken ct);

    Task<TriggeredJobWithTasksModel[]> RetryJobs(EventKey eventKey, JobKey jobKey, CancellationToken ct);

    Task<TriggeredJobWithTasksModel> StartJob
    (
        int jobID,
        NextTaskModel[] nextTasks,
        CancellationToken ct
    );

    Task StartTask(int taskID, CancellationToken ct);

    Task JobCancelled(int taskID, string reason, CancellationToken ct);
    
    Task<TriggeredJobWithTasksModel> TaskCompleted
    (
        int completedTaskID,
        bool preserveData,
        NextTaskModel[] nextTasks,
        CancellationToken ct
    );

    Task<TriggeredJobWithTasksModel> TaskFailed
    (
        int failedTaskID,
        JobTaskStatus errorStatus,
        TimeSpan retryAfter,
        NextTaskModel[] nextTasks,
        string category,
        string message,
        string detail,
        string sourceLogEntryKey,
        CancellationToken ct
    );

    Task LogMessage
    (
        int taskID,
        string category,
        string message,
        string details,
        CancellationToken ct
    );
}