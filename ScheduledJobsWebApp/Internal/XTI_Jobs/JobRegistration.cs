namespace XTI_Jobs;

public sealed class JobRegistration
{
    private readonly IStoredEvents storedEvents;
    private readonly List<JobBuilder> jobs = new();

    public JobRegistration(IStoredEvents storedEvents)
    {
        this.storedEvents = storedEvents;
    }

    public JobRegistration AddJob(JobKey jobKey, Action<JobBuilder> config)
    {
        var job = new JobBuilder(jobKey);
        jobs.Add(job);
        config(job);
        return this;
    }

    public Task Register()
    {
        var registeredJobs = jobs.Select(j => j.Build()).ToArray();
        return storedEvents.AddOrUpdateRegisteredJobs(registeredJobs);
    }
}
