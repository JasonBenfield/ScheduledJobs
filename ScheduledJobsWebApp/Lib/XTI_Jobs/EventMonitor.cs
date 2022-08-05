namespace XTI_Jobs;

public sealed class EventMonitor
{
    private readonly IJobDb db;
    private readonly IJobActionFactory jobActionFactory;
    private readonly EventKey eventKey;
    private readonly JobKey jobKey;
    private readonly DateTimeOffset eventRaisedStartTime;

    internal EventMonitor(IJobDb db, IJobActionFactory jobActionFactory, EventKey eventKey, JobKey jobKey, DateTimeOffset eventRaisedStartTime)
    {
        this.db = db;
        this.jobActionFactory = jobActionFactory;
        this.eventKey = eventKey;
        this.jobKey = jobKey;
        this.eventRaisedStartTime = eventRaisedStartTime;
    }

    public async Task Run(CancellationToken stoppingToken)
    {
        var retryJobs = await db.RetryJobs(jobKey);
        foreach(var retryJob in retryJobs)
        {
            var triggeredJob = new TriggeredJob(db, retryJob);
            var nextTask = await triggeredJob.StartNextTask();
            while (nextTask != null)
            {
                nextTask = await ExecuteTask(stoppingToken, triggeredJob, nextTask);
            }
        }
        var pendingJobs = await db.TriggerJobs(eventKey, jobKey, eventRaisedStartTime);
        foreach (var pendingJob in pendingJobs)
        {
            var transformedData = jobActionFactory.CreateTransformedSourceData(pendingJob.SourceData);
            var taskData = await transformedData.Value();
            var firstTasks = jobActionFactory.FirstTasks(taskData);
            var triggeredJob = new TriggeredJob(db, pendingJob);
            var nextTask = await triggeredJob.Start(firstTasks);
            while (nextTask != null)
            {
                nextTask = await ExecuteTask(stoppingToken, triggeredJob, nextTask);
            }
        }
    }

    private async Task<TriggeredJobTask?> ExecuteTask(CancellationToken stoppingToken, TriggeredJob triggeredJob, TriggeredJobTask currentTask)
    {
        TriggeredJobTask? nextTask;
        var jobAction = jobActionFactory.CreateJobAction(currentTask);
        JobActionResult result;
        try
        {
            result = await jobAction.Execute(stoppingToken);
            await currentTask.Completed(result.PreserveData, result.NextTasks);
            nextTask = await triggeredJob.StartNextTask();
        }
        catch(CancelJobException cancelJobEx)
        {
            await currentTask.CancelJob(cancelJobEx.Reason);
            nextTask = null;
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
