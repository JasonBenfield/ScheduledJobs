﻿namespace XTI_ScheduledJobsAppClient;

public sealed class SjcJobDb : IJobDb
{
    private readonly ScheduledJobsAppClient schdJobClient;

    public SjcJobDb(ScheduledJobsAppClient schdJobClient)
    {
        this.schdJobClient = schdJobClient;
    }

    public Task<EventNotificationModel[]> AddEventNotifications(EventKey eventKey, EventSource[] sources) =>
        schdJobClient.Events.AddNotifications
        (
            new AddNotificationsRequest
            {
                EventKey = eventKey,
                Sources = sources
            }
        );

    public Task AddOrUpdateRegisteredEvents(RegisteredEvent[] registeredEvents) =>
        schdJobClient.Events.AddOrUpdateRegisteredEvents(registeredEvents);

    public Task AddOrUpdateRegisteredJobs(RegisteredJob[] registeredJobs) =>
        schdJobClient.Jobs.AddOrUpdateRegisteredJobs(registeredJobs);

    public Task LogMessage(int taskID, string category, string message, string details) =>
        schdJobClient.Jobs.LogMessage
        (
            new LogMessageRequest
            {
                TaskID = taskID,
                Category = category,
                Message = message,
                Details = details
            }
        );

    public Task<TriggeredJobDetailModel[]> RetryJobs(JobKey jobKey) =>
        schdJobClient.Jobs.RetryJobs
        (
            new RetryJobsRequest
            {
                JobKey = jobKey
            }
        );

    public Task<TriggeredJobDetailModel> StartJob(int jobID, NextTaskModel[] nextTasks) =>
        schdJobClient.Jobs.StartJob
        (
            new StartJobRequest
            {
                JobID = jobID,
                NextTasks = nextTasks
            }
        );

    public Task StartTask(int taskID) =>
        schdJobClient.Jobs.StartTask
        (
            new StartTaskRequest
            {
                TaskID = taskID
            }
        );

    public Task<TriggeredJobDetailModel> TaskCompleted
    (
        int jobID,
        int completedTaskID,
        NextTaskModel[] nextTasks
    ) =>
        schdJobClient.Jobs.TaskCompleted
        (
            new TaskCompletedRequest
            {
                JobID = jobID,
                CompletedTaskID = completedTaskID,
                NextTasks = nextTasks
            }
        );

    public Task<TriggeredJobDetailModel> TaskFailed
    (
        int jobID,
        int failedTaskID,
        JobTaskStatus errorStatus,
        TimeSpan retryAfter,
        NextTaskModel[] nextTasks,
        string category,
        string message,
        string detail
    ) =>
        schdJobClient.Jobs.TaskFailed
        (
            new TaskFailedRequest
            {
                JobID = jobID,
                FailedTaskID = failedTaskID,
                ErrorStatus = errorStatus,
                RetryAfter = retryAfter,
                NextTasks = nextTasks,
                Category = category,
                Message = message,
                Detail = detail
            }
        );

    public Task<TriggeredJobDetailModel[]> TriggeredJobs(int notificationID) =>
        schdJobClient.Events.TriggeredJobs
        (
            new TriggeredJobsRequest
            {
                EventNotificationID = notificationID
            }
        );

    public Task<PendingJobModel[]> TriggerJobs(EventKey eventKey, JobKey jobKey) =>
        schdJobClient.Jobs.TriggerJobs
        (
            new TriggerJobsRequest
            {
                EventKey = eventKey,
                JobKey = jobKey
            }
        );
}