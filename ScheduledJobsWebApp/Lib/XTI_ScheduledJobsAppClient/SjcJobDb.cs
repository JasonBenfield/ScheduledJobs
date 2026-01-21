using XTI_Schedule;

namespace XTI_ScheduledJobsAppClient;

public sealed class SjcJobDb : IJobDb
{
    private readonly ScheduledJobsAppClient schdJobClient;

    public SjcJobDb(ScheduledJobsAppClient schdJobClient)
    {
        this.schdJobClient = schdJobClient;
    }

    public Task AddOrUpdateJobSchedules(JobKey jobKey, AggregateSchedule aggregateSchedule, TimeSpan deleteAfter, CancellationToken ct) =>
        schdJobClient.Jobs.AddOrUpdateJobSchedules
        (
            new AddOrUpdateJobSchedulesRequest
            {
                JobKey = jobKey.DisplayText,
                Schedules = aggregateSchedule.Serialize(),
                DeleteAfter = deleteAfter
            },
            ct
        );

    public Task<EventNotificationModel[]> AddEventNotifications(EventKey eventKey, XtiEventSource[] sources, CancellationToken ct) =>
        schdJobClient.Events.AddNotifications
        (
            new AddNotificationsRequest(eventKey, sources),
            ct
        );

    public Task AddOrUpdateRegisteredEvents(RegisteredEvent[] registeredEvents, CancellationToken ct) =>
        schdJobClient.Events.AddOrUpdateRegisteredEvents(registeredEvents, ct);

    public Task AddOrUpdateRegisteredJobs(RegisteredJob[] registeredJobs, CancellationToken ct) =>
        schdJobClient.Jobs.AddOrUpdateRegisteredJobs(registeredJobs, ct);

    public Task LogMessage(int taskID, string category, string message, string details, CancellationToken ct) =>
        schdJobClient.Jobs.LogMessage
        (
            new LogMessageRequest
            {
                TaskID = taskID,
                Category = category,
                Message = message,
                Details = details
            },
            ct
        );

    public Task DeleteJobsWithNoTasks(EventKey eventKey, JobKey jobKey, CancellationToken ct) =>
        schdJobClient.Jobs.DeleteJobsWithNoTasks
        (
            new DeleteJobsWithNoTasksRequest(eventKey, jobKey),
            ct
        );

    public Task<TriggeredJobWithTasksModel[]> RetryJobs(EventKey eventKey, JobKey jobKey, CancellationToken ct) =>
        schdJobClient.Jobs.RetryJobs
        (
            new RetryJobsRequest(eventKey, jobKey),
            ct
        );

    public Task<TriggeredJobWithTasksModel> StartJob(int jobID, NextTaskModel[] nextTasks, CancellationToken ct) =>
        schdJobClient.Jobs.StartJob
        (
            new StartJobRequest(jobID, nextTasks),
            ct
        );

    public Task StartTask(int taskID, CancellationToken ct) =>
        schdJobClient.Jobs.StartTask
        (
            new StartTaskRequest(taskID),
            ct
        );

    public Task JobCancelled(int taskID, string reason, CancellationToken ct) =>
        schdJobClient.Jobs.JobCancelled
        (
            new JobCancelledRequest(taskID, reason),
            ct
        );

    public Task<TriggeredJobWithTasksModel> TaskCompleted
    (
        int completedTaskID,
        bool preserveData,
        NextTaskModel[] nextTasks,
        CancellationToken ct
    ) =>
        schdJobClient.Jobs.TaskCompleted
        (
            new TaskCompletedRequest(completedTaskID, preserveData, nextTasks),
            ct
        );

    public Task<TriggeredJobWithTasksModel> TaskFailed
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
    ) =>
        schdJobClient.Jobs.TaskFailed
        (
            new TaskFailedRequest
            {
                FailedTaskID = failedTaskID,
                ErrorStatus = errorStatus,
                RetryAfter = retryAfter,
                NextTasks = nextTasks,
                Category = category,
                Message = message,
                Detail = detail,
                SourceLogEntryKey = sourceLogEntryKey
            },
            ct
        );

    public Task<TriggeredJobWithTasksModel[]> TriggeredJobs(int notificationID, CancellationToken ct) =>
        schdJobClient.Events.TriggeredJobs
        (
            new TriggeredJobsRequest
            {
                EventNotificationID = notificationID
            },
            ct
        );

    public Task<PendingJobModel[]> TriggerJobs(EventKey eventKey, JobKey jobKey, DateTimeOffset eventRaisedStartTime, CancellationToken ct) =>
        schdJobClient.Jobs.TriggerJobs
        (
            new TriggerJobsRequest(eventKey, jobKey, eventRaisedStartTime),
            ct
        );

}
