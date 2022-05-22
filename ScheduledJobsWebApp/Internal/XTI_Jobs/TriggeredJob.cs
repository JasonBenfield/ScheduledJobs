using XTI_Core;

namespace XTI_Jobs;

public sealed class TriggeredJob
{
    private readonly IJobDb db;
    private readonly IClock clock;
    private TriggeredJobTask[] tasks = new TriggeredJobTask[0];

    internal TriggeredJob(IJobDb db, IClock clock, PendingJobModel job)
        : this
        (
            db,
            clock,
            new TriggeredJobDetailModel
            (
                new TriggeredJobModel(job.Job.ID, job.Job.JobDefinition),
                new TriggeredJobTaskModel[0]
            )
        )
    {
    }

    public TriggeredJob(IJobDb db, IClock clock, TriggeredJobDetailModel jobDetail)
    {
        this.db = db;
        this.clock = clock;
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

    internal async Task<TriggeredJobTask?> Start(NextTaskModel[] firstTasks)
    {
        var startedJob = await db.StartJob(Model, firstTasks, clock.Now());
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
                await db.StartTask(nextTask.Model, clock.Now());
            }
        }
        return nextTask;
    }

    internal Task LogMessage(TriggeredJobTask task, string category, string message, string details) =>
        db.LogMessage
        (
            task.Model,
            category,
            message,
            details,
            clock.Now()
        );

    internal async Task<TriggeredJobTask?> TaskFailed(TriggeredJobTask task, JobTaskStatus errorStatus, TimeSpan retryAfter, NextTaskModel[] nextTasks, Exception ex)
    {
        var updatedJob = await db.TaskFailed
        (
            Model,
            task.Model,
            errorStatus,
            retryAfter,
            nextTasks,
            ex.GetType().Name,
            ex.Message,
            ex.ToString(),
            clock.Now()
        );
        UpdateJob(updatedJob);
        var nextTask = await StartNextTask();
        return nextTask;
    }

    internal async Task TaskCompleted(TriggeredJobTask task, NextTaskModel[] nextTasks)
    {
        var updatedJob = await db.TaskCompleted(Model, task.Model, nextTasks, clock.Now());
        UpdateJob(updatedJob);
    }

    private void UpdateJob(TriggeredJobDetailModel jobDetail)
    {
        Model = jobDetail.Job;
        tasks = jobDetail.Tasks.Select(t => new TriggeredJobTask(this, t)).ToArray();
    }

}
