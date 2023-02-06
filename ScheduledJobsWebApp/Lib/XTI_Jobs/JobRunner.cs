namespace XTI_Jobs;

internal sealed class JobRunner
{
    private readonly IJobDb db;
    private readonly IJobActionFactory jobActionFactory;

    public JobRunner(IJobDb db, IJobActionFactory jobActionFactory)
    {
        this.db = db;
        this.jobActionFactory = jobActionFactory;
    }

    public async Task<TriggeredJob> StartRetry(TriggeredJobWithTasksModel retryJob, CancellationToken stoppingToken)
    {
        var triggeredJob = new TriggeredJob(db, retryJob);
        var nextTask = await triggeredJob.StartNextTask();
        while (nextTask != null)
        {
            nextTask = await ExecuteTask(stoppingToken, triggeredJob, nextTask);
        }
        return triggeredJob;
    }

    public async Task<TriggeredJob> StartNew(PendingJobModel pendingJob, string taskData, CancellationToken stoppingToken)
    {
        var firstTasks = jobActionFactory.FirstTasks(taskData);
        var triggeredJob = new TriggeredJob(db, pendingJob);
        var nextTask = await triggeredJob.Start(firstTasks);
        while (nextTask != null)
        {
            nextTask = await ExecuteTask(stoppingToken, triggeredJob, nextTask);
        }
        return triggeredJob;
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
        catch (CancelJobException cancelJobEx)
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
