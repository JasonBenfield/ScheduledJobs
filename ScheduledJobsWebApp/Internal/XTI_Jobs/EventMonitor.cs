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
        var retryJobs = await db.Retry(jobKey, clock.Now());
        foreach(var retryJob in retryJobs)
        {
            var triggeredJob = new TriggeredJob(db, clock, retryJob);
            var nextTask = await triggeredJob.StartNextTask();
            while (nextTask != null)
            {
                nextTask = await ExecuteTask(triggeredJob, nextTask);
            }
        }
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
        }
    }

    private async Task<TriggeredJobTask?> ExecuteTask(TriggeredJob triggeredJob, TriggeredJobTask currentTask)
    {
        TriggeredJobTask? nextTask;
        var jobAction = jobActionFactory.CreateJobAction(currentTask);
        JobActionResult result;
        try
        {
            result = await jobAction.Execute();
            await currentTask.Completed(result.NextTasks);
            nextTask = await triggeredJob.StartNextTask();
        }
        catch (Exception ex)
        {
            JobErrorResult errorResult;
            try
            {
                errorResult = await jobAction.OnError(ex);
            }
            catch
            {
                errorResult = new JobErrorResultBuilder(currentTask.Model).Build();
            }
            nextTask = await currentTask.Failed
            (
                errorResult.UpdatedStatus, 
                errorResult.RetryAfter, 
                errorResult.NextTasks, 
                ex
            );
        }
        return nextTask;
    }
}
