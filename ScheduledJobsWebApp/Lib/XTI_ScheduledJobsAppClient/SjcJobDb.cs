﻿using XTI_Schedule;

namespace XTI_ScheduledJobsAppClient;

public sealed class SjcJobDb : IJobDb
{
    private readonly ScheduledJobsAppClient schdJobClient;

    public SjcJobDb(ScheduledJobsAppClient schdJobClient)
    {
        this.schdJobClient = schdJobClient;
    }

    public Task AddOrUpdateJobSchedules(JobKey jobKey, AggregateSchedule aggregateSchedule, TimeSpan deleteAfter) =>
        schdJobClient.Jobs.AddOrUpdateJobSchedules
        (
            new AddOrUpdateJobSchedulesRequest
            {
                JobKey = jobKey.DisplayText,
                Schedules = aggregateSchedule.Serialize(),
                DeleteAfter = deleteAfter
            }
        );

    public Task<EventNotificationModel[]> AddEventNotifications(EventKey eventKey, XtiEventSource[] sources) =>
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

    public Task DeleteJobsWithNoTasks(EventKey eventKey, JobKey jobKey) =>
        schdJobClient.Jobs.DeleteJobsWithNoTasks
        (
            new DeleteJobsWithNoTasksRequest
            {
                EventKey = eventKey,
                JobKey = jobKey
            }
        );

    public Task<TriggeredJobWithTasksModel[]> RetryJobs(EventKey eventKey, JobKey jobKey) =>
        schdJobClient.Jobs.RetryJobs
        (
            new RetryJobsRequest
            {
                EventKey = eventKey,
                JobKey = jobKey
            }
        );

    public Task<TriggeredJobWithTasksModel> StartJob(int jobID, NextTaskModel[] nextTasks) =>
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

    public Task JobCancelled(int taskID, string reason) =>
        schdJobClient.Jobs.JobCancelled
        (
            new JobCancelledRequest
            {
                TaskID = taskID,
                Reason = reason
            }
        );

    public Task<TriggeredJobWithTasksModel> TaskCompleted
    (
        int completedTaskID,
        bool preserveData,
        NextTaskModel[] nextTasks
    ) =>
        schdJobClient.Jobs.TaskCompleted
        (
            new TaskCompletedRequest
            {
                CompletedTaskID = completedTaskID,
                PreserveData = preserveData,
                NextTasks = nextTasks
            }
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
        string sourceLogEntryKey
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
            }
        );

    public Task<TriggeredJobWithTasksModel[]> TriggeredJobs(int notificationID) =>
        schdJobClient.Events.TriggeredJobs
        (
            new TriggeredJobsRequest
            {
                EventNotificationID = notificationID
            }
        );

    public Task<PendingJobModel[]> TriggerJobs(EventKey eventKey, JobKey jobKey, DateTimeOffset eventRaisedStartTime) =>
        schdJobClient.Jobs.TriggerJobs
        (
            new TriggerJobsRequest
            {
                EventKey = eventKey,
                JobKey = jobKey,
                EventRaisedStartTime = eventRaisedStartTime
            }
        );

}
