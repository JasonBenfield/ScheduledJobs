using XTI_Core;

namespace XTI_Jobs.Abstractions;

public interface IJobDb
{
    Task AddOrUpdateRegisteredEvents(RegisteredEvent[] registeredEvents);

    Task AddOrUpdateRegisteredJobs(RegisteredJob[] registeredJobs);

    Task<EventNotificationModel[]> AddNotifications(EventKey eventKey, EventSource[] sources, DateTimeOffset now);

    Task<PendingJobModel[]> TriggerJobs(EventKey eventKey, JobKey jobKey, DateTimeOffset now);

    Task<TriggeredJobDetailModel[]> TriggeredJobs(EventNotificationModel notification);

    Task<TriggeredJobDetailModel> StartJob(TriggeredJobModel pendingJob, NextTaskModel[] nextTasks, DateTimeOffset now);

    Task StartTask(TriggeredJobTaskModel pendingTask, DateTimeOffset now);

    Task<TriggeredJobDetailModel> TaskCompleted(TriggeredJobModel pendingJob, TriggeredJobTaskModel currentTask, NextTaskModel[] nextTasks, DateTimeOffset now);

    Task JobCompleted(PendingJobModel job);

    Task<TriggeredJobDetailModel> TaskFailed(TriggeredJobModel job, TriggeredJobTaskModel task, AppEventSeverity severity, string category, string message, string detail);
}