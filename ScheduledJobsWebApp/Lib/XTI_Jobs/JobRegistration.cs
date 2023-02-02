namespace XTI_Jobs;

public sealed class JobRegistration
{
    private readonly IJobDb db;
    private readonly JobRegistrationBuilder1[] jobs;

    internal JobRegistration(IJobDb db, JobRegistrationBuilder1[] jobs)
    {
        this.db = db;
        this.jobs = jobs;
    }

    public Task Register()
    {
        var registeredJobs = jobs.Select(j => j.BuildJob()).ToArray();
        return db.AddOrUpdateRegisteredJobs(registeredJobs);
    }
}