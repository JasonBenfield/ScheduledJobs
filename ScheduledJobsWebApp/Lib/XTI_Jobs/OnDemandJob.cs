namespace XTI_Jobs;

public sealed class OnDemandJob
{
    private readonly IJobDb db;
    private readonly JobKey jobKey;
    private readonly IJobActionFactory jobActionFactory;
    private readonly string[] data;
    private TimeSpan deleteAfter;

    internal OnDemandJob(IJobDb db, JobKey jobKey, IJobActionFactory jobActionFactory, string[] data, TimeSpan deleteAfter)
    {
        this.db = db;
        this.jobKey = jobKey;
        this.jobActionFactory = jobActionFactory;
        this.data = data;
        this.deleteAfter = deleteAfter;
    }

    public async Task<TriggeredJob[]> RunUntilCompletion(CancellationToken stoppingToken)
    {
        var triggeredJobs = await Run(stoppingToken);
        while (!triggeredJobs.All(j => !j.Status().EqualsAny(JobTaskStatus.Values.Pending, JobTaskStatus.Values.Running)))
        {
            await Task.Delay(500);
        }
        return triggeredJobs;
    }

    public async Task<TriggeredJob[]> Run(CancellationToken stoppingToken)
    {
        var eventKey = EventKey.OnDemand(jobKey);
        await db.AddOrUpdateRegisteredEvents
        (
            new[]
            {
                new RegisteredEvent
                (
                    eventKey,
                    false,
                    DuplicateHandling.Values.KeepAll,
                    DateTimeOffset.MinValue,
                    TimeSpan.FromMinutes(5),
                    deleteAfter
                )
            }
        );
        var sources = data.Select(d => new XtiEventSource("", d)).ToArray();
        var notifications = await db.AddEventNotifications(eventKey, sources);
        var pendingJobs = await db.TriggerJobs(eventKey, jobKey, DateTimeOffset.MinValue);
        var jobRunner = new JobRunner(db, jobActionFactory);
        var triggeredJobs = new List<TriggeredJob>();
        foreach (var pendingJob in pendingJobs)
        {
            var triggeredJob = await jobRunner.StartNew(pendingJob, pendingJob.SourceData, stoppingToken);
            triggeredJobs.Add(triggeredJob);
        }
        return triggeredJobs.ToArray();
    }
}
