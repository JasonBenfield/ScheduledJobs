namespace XTI_Jobs.Abstractions;

public interface IJobDb
{
    Task AddOrUpdateRegisteredEvents(RegisteredEvent[] registeredEvents);

    Task AddOrUpdateRegisteredJobs(RegisteredJob[] registeredJobs);

    Task<EventNotificationModel[]> AddEventNotifications(EventKey eventKey, EventSource[] sources);

    Task<TriggeredJobWithTasksModel[]> TriggeredJobs(int notificationID);

    Task<PendingJobModel[]> TriggerJobs(EventKey eventKey, JobKey jobKey, DateTimeOffset eventRaisedStartTime);

    Task DeleteJobsWithNoTasks(JobKey jobKey);

    Task<TriggeredJobWithTasksModel[]> RetryJobs(JobKey jobKey);

    Task<TriggeredJobWithTasksModel> StartJob
    (
        int jobID,
        NextTaskModel[] nextTasks
    );

    Task StartTask(int taskID);

    Task JobCancelled(int taskID, string reason);
    
    Task<TriggeredJobWithTasksModel> TaskCompleted
    (
        int completedTaskID,
        bool preserveData,
        NextTaskModel[] nextTasks
    );

    Task<TriggeredJobWithTasksModel> TaskFailed
    (
        int failedTaskID,
        JobTaskStatus errorStatus,
        TimeSpan retryAfter,
        NextTaskModel[] nextTasks,
        string category,
        string message,
        string detail
    );

    Task LogMessage
    (
        int taskID,
        string category,
        string message,
        string details
    );
}