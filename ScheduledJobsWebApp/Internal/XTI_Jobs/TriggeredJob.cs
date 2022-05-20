using XTI_Core;

namespace XTI_Jobs;

public sealed class TriggeredJob
{
    private readonly IJobDb db;
    private readonly IClock clock;
    private TriggeredJobTask[] tasks = new TriggeredJobTask[0];

    public TriggeredJob(IJobDb db, IClock clock, PendingJobModel job)
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

    public async Task<TriggeredJobTask?> Start(NextTaskModel[] firstTasks)
    {
        var startedJob = await db.StartJob(Model, firstTasks, clock.Now());
        UpdateJob(startedJob);
        var task = await StartNextTask();
        return task;
    }

    public async Task<TriggeredJobTask?> StartNextTask()
    {
        var nextTask = tasks.Where(t => t.Model.Status.Equals(JobTaskStatus.Values.Pending)).FirstOrDefault();
        if (nextTask != null)
        {
            await db.StartTask(nextTask.Model, clock.Now());
        }
        return nextTask;
    }

    internal async Task TaskFailed(TriggeredJobTask task, Exception ex)
    {
        var updatedJob = await db.TaskFailed
        (
            Model, 
            task.Model, 
            AppEventSeverity.Values.CriticalError, 
            ex.GetType().Name, 
            ex.Message, 
            ex.ToString()
        );
        UpdateJob(updatedJob);
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
