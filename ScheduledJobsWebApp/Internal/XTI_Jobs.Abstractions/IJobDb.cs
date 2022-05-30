namespace XTI_Jobs.Abstractions;

public interface IJobDb
{
    Task AddOrUpdateRegisteredEvents(RegisteredEvent[] registeredEvents);

    Task AddOrUpdateRegisteredJobs(RegisteredJob[] registeredJobs);

    Task<EventNotificationModel[]> AddEventNotifications(EventKey eventKey, EventSource[] sources);

    Task<TriggeredJobDetailModel[]> TriggeredJobs(int notificationID);

    Task<PendingJobModel[]> TriggerJobs(EventKey eventKey, JobKey jobKey);

    Task<TriggeredJobDetailModel[]> RetryJobs(JobKey jobKey);

    Task<TriggeredJobDetailModel> StartJob
    (
        int jobID, 
        NextTaskModel[] nextTasks
    );

    Task StartTask(int taskID);

    Task<TriggeredJobDetailModel> TaskCompleted
    (
        int jobID,
        int completedTaskID, 
        NextTaskModel[] nextTasks
    );

    Task<TriggeredJobDetailModel> TaskFailed
    (
        int jobID, 
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