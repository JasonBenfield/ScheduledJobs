namespace XTI_Jobs;

public sealed class TriggeredJob
{
    private readonly IJobDb db;
    private TriggeredJobTask[] tasks = new TriggeredJobTask[0];

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
                new TriggeredJobTaskModel[0]
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

    public LogEntryModel[] Errors() => tasks.SelectMany(t => t.Errors()).ToArray();

    public LogEntryModel[] Messages() => tasks.SelectMany(t => t.Messages()).ToArray();

    public JobTaskStatus Status() => tasks
        .Select(t => t.Model.Status)
        .Where(st => !st.Equals(JobTaskStatus.Values.NotSet))
        .OrderBy(st => st.Value)
        .FirstOrDefault()
        ?? JobTaskStatus.Values.NotSet;

    internal Task CancelJob(TriggeredJobTask triggeredJobTask, string reason, DeletionTime deletionTime) =>
        db.JobCancelled(triggeredJobTask.Model.ID, reason, deletionTime);

    internal async Task<TriggeredJobTask?> Start(NextTaskModel[] firstTasks)
    {
        var startedJob = await db.StartJob(Model.ID, firstTasks);
        UpdateJob(startedJob);
        var task = await StartNextTask();
        return task;
    }

    internal async Task<TriggeredJobTask?> StartNextTask()
    {
        TriggeredJobTask? nextTask = null;
        var anyErrors = tasks.Any(t => t.Model.Status.EqualsAny(JobTaskStatus.Values.Failed, JobTaskStatus.Values.Retry));
        if (!anyErrors)
        {
            nextTask = tasks.Where(t => t.Model.Status.Equals(JobTaskStatus.Values.Pending)).FirstOrDefault();
            if (nextTask != null)
            {
                await db.StartTask(nextTask.Model.ID);
            }
        }
        return nextTask;
    }

    internal Task LogMessage(TriggeredJobTask task, string category, string message, string details) =>
        db.LogMessage
        (
            task.Model.ID,
            category,
            message,
            details
        );

    internal async Task<TriggeredJobTask?> TaskFailed(TriggeredJobTask task, JobTaskStatus errorStatus, TimeSpan retryAfter, NextTaskModel[] nextTasks, Exception ex)
    {
        var updatedJob = await db.TaskFailed
        (
            task.Model.ID,
            errorStatus,
            retryAfter,
            nextTasks,
            ex.GetType().Name,
            ex.Message,
            ex.ToString()
        );
        UpdateJob(updatedJob);
        var nextTask = await StartNextTask();
        return nextTask;
    }

    internal async Task TaskCompleted(TriggeredJobTask task, bool preserveData, NextTaskModel[] nextTasks)
    {
        var updatedJob = await db.TaskCompleted(task.Model.ID, preserveData, nextTasks);
        UpdateJob(updatedJob);
    }

    private void UpdateJob(TriggeredJobWithTasksModel jobDetail)
    {
        Model = jobDetail.Job;
        tasks = jobDetail.Tasks.Select(t => new TriggeredJobTask(this, t)).ToArray();
    }

}
