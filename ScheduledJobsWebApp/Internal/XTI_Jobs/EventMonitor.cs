using XTI_Core;

namespace XTI_Jobs;

public sealed class EventMonitor
{
    private readonly IJobDb db;
    private readonly IJobActionFactory jobActionFactory;
    private readonly IClock clock;
    private readonly EventKey eventKey;
    private readonly JobKey jobKey;

    internal EventMonitor(IJobDb db, IJobActionFactory jobActionFactory, IClock clock, EventKey eventKey, JobKey jobKey)
    {
        this.db = db;
        this.jobActionFactory = jobActionFactory;
        this.clock = clock;
        this.eventKey = eventKey;
        this.jobKey = jobKey;
    }

    public async Task Run()
    {
        var pendingJobs = await db.TriggerJobs(eventKey, jobKey, clock.Now());
        foreach (var pendingJob in pendingJobs)
        {
            var transformedData = jobActionFactory.CreateTransformedSourceData(pendingJob.SourceData);
            var taskData = await transformedData.Value();
            var firstTasks = jobActionFactory.FirstTasks(taskData);
            var triggeredJob = new TriggeredJob(db, clock, pendingJob);
            var nextTask = await triggeredJob.Start(firstTasks);
            while (nextTask != null)
            {
                nextTask = await ExecuteTask(triggeredJob, nextTask);
            }
            await db.JobCompleted(pendingJob);
        }
    }

    private async Task<TriggeredJobTask?> ExecuteTask(TriggeredJob triggeredJob, TriggeredJobTask currentTask)
    {
        var jobAction = jobActionFactory.CreateJobAction(currentTask);
        JobActionResult result;
        try
        {
            result = await jobAction.Execute();
            await currentTask.Completed(result.NextTasks);
        }
        catch (Exception ex)
        {
            await currentTask.Failed(ex);
        }
        var nextTask = await triggeredJob.StartNextTask();
        return nextTask;
    }
}
