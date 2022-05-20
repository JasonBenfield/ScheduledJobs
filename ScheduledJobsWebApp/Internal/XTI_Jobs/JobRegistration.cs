namespace XTI_Jobs;

public sealed class JobRegistration
{
    private readonly IJobDb db;
    private readonly List<JobBuilder> jobs = new();

    public JobRegistration(IJobDb db)
    {
        this.db = db;
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
        return db.AddOrUpdateRegisteredJobs(registeredJobs);
    }
}
