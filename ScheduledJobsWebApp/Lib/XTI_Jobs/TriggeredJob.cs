using XTI_App.Abstractions;

namespace XTI_Jobs;

public sealed class TriggeredJob
{
    private readonly IJobDb db;
    private TriggeredJobTask[] tasks = [];

    internal TriggeredJob(IJobDb db, PendingJobModel pendingJob)
        : this
        (
            db,
            new TriggeredJobWithTasksModel
            (
                new TriggeredJobModel
                (
                    pendingJob.Job.ID,
                    pendingJob.Job.JobDefinition,
                    pendingJob.Job.EventNotificationID
                ),
                []
            )
        )
    {
    }

    public TriggeredJob(IJobDb db, TriggeredJobWithTasksModel jobDetail)
    {
        this.db = db;
        Model = jobDetail.Job;
        UpdateJob(jobDetail);
    }

    public TriggeredJobModel Model { get; private set; }

    public TriggeredJobTask Task(TriggeredJobTaskModel task) => Task(task.ID);

    public TriggeredJobTask Task(int id) =>
        tasks
            .Where(t => t.Model.ID == id)
            .First();

    public TriggeredJobTask[] Tasks(JobTaskKey taskKey) =>
        tasks
            .Where(t => t.Model.TaskDefinition.TaskKey == taskKey.Value)
            .ToArray();

    public TriggeredJobTask[] Tasks() => tasks.ToArray();

    public JobLogEntryModel[] Errors() => tasks.SelectMany(t => t.Errors()).ToArray();

    public JobLogEntryModel[] Messages() => tasks.SelectMany(t => t.Messages()).ToArray();

    public JobTaskStatus Status() => tasks
        .Select(t => t.Model.Status)
        .Where(st => !st.Equals(JobTaskStatus.Values.NotSet))
        .OrderBy(st => st.Value)
        .FirstOrDefault()
        ?? JobTaskStatus.Values.NotSet;

    internal Task CancelJob(TriggeredJobTask triggeredJobTask, string reason, CancellationToken ct) =>
        db.JobCancelled(triggeredJobTask.Model.ID, reason, ct);

    internal async Task<TriggeredJobTask?> Start(NextTaskModel[] firstTasks, CancellationToken ct)
    {
        var startedJob = await db.StartJob(Model.ID, firstTasks, ct);
        UpdateJob(startedJob);
        var task = await StartNextTask(ct);
        return task;
    }

    internal async Task<TriggeredJobTask?> StartNextTask(CancellationToken ct)
    {
        TriggeredJobTask? nextTask = null;
        var anyErrors = tasks.Any(t => t.Model.Status.EqualsAny(JobTaskStatus.Values.Failed, JobTaskStatus.Values.Retry));
        if (!anyErrors)
        {
            nextTask = tasks.Where(t => t.Model.Status.Equals(JobTaskStatus.Values.Pending)).FirstOrDefault();
            if (nextTask != null)
            {
                await db.StartTask(nextTask.Model.ID, ct);
            }
        }
        return nextTask;
    }

    internal Task LogMessage(TriggeredJobTask task, string category, string message, string details, CancellationToken ct) =>
        db.LogMessage
        (
            task.Model.ID,
            category,
            message,
            details,
            ct
        );

    internal async Task<TriggeredJobTask?> TaskFailed(TriggeredJobTask task, JobTaskStatus errorStatus, TimeSpan retryAfter, NextTaskModel[] nextTasks, Exception ex, CancellationToken ct)
    {
        var clientException = ex as AppClientException;
        var updatedJob = await db.TaskFailed
        (
            task.Model.ID,
            errorStatus,
            retryAfter,
            nextTasks,
            ex.GetType().Name,
            ex.Message,
            ex.ToString(),
            clientException?.LogEntryKey ?? "",
            ct
        );
        UpdateJob(updatedJob);
        var nextTask = await StartNextTask(ct);
        return nextTask;
    }

    internal async Task TaskCompleted(TriggeredJobTask task, bool preserveData, NextTaskModel[] nextTasks, CancellationToken ct)
    {
        var updatedJob = await db.TaskCompleted(task.Model.ID, preserveData, nextTasks, ct);
        UpdateJob(updatedJob);
    }

    private void UpdateJob(TriggeredJobWithTasksModel jobDetail)
    {
        Model = jobDetail.Job;
        tasks = jobDetail.Tasks.Select(t => new TriggeredJobTask(this, t)).ToArray();
    }

}
