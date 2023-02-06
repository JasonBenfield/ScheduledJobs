namespace XTI_Jobs;

public sealed class EventMonitor
{
    private readonly IJobDb db;
    private readonly IJobActionFactory jobActionFactory;
    private readonly ITransformedEventData transformedEventData;
    private readonly EventKey eventKey;
    private readonly JobKey jobKey;
    private readonly DateTimeOffset eventRaisedStartTime;

    internal EventMonitor(IJobDb db, IJobActionFactory jobActionFactory, ITransformedEventData transformedEventData, EventKey eventKey, JobKey jobKey, DateTimeOffset eventRaisedStartTime)
    {
        this.db = db;
        this.jobActionFactory = jobActionFactory;
        this.transformedEventData = transformedEventData;
        this.eventKey = eventKey;
        this.jobKey = jobKey;
        this.eventRaisedStartTime = eventRaisedStartTime;
    }

    public async Task<TriggeredJob[]> Run(CancellationToken stoppingToken)
    {
        if (eventKey.IsOnDemand())
        {
            throw new ArgumentException($"Unable to monitor on demand job '{eventKey.DisplayText}'");
        }
        await db.DeleteJobsWithNoTasks(eventKey, jobKey);
        var retryJobs = await db.RetryJobs(eventKey, jobKey);
        var jobRunner = new JobRunner(db, jobActionFactory);
        var triggeredJobs = new List<TriggeredJob>();
        foreach(var retryJob in retryJobs)
        {
            var triggeredJob = await jobRunner.StartRetry(retryJob, stoppingToken);
            triggeredJobs.Add(triggeredJob);
        }
        var pendingJobs = await db.TriggerJobs(eventKey, jobKey, eventRaisedStartTime);
        foreach (var pendingJob in pendingJobs)
        {
            var taskData = await transformedEventData.TransformEventData(pendingJob.SourceKey, pendingJob.SourceData);
            var triggeredJob = await jobRunner.StartNew(pendingJob, taskData, stoppingToken);
            triggeredJobs.Add(triggeredJob);
        }
        return triggeredJobs.ToArray();
    }
}
