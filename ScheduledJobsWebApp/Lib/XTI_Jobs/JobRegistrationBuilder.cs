namespace XTI_Jobs;

public sealed class JobRegistrationBuilder
{
    private readonly IJobDb db;
    private readonly List<JobRegistrationBuilder1> jobs = new();

    public JobRegistrationBuilder(IJobDb db)
    {
        this.db = db;
    }

    public JobRegistrationBuilder1 AddJob(JobKey jobKey)
    {
        var job = new JobRegistrationBuilder1(this, jobKey);
        jobs.Add(job);
        return job;
    }

    internal JobRegistration Build() => new JobRegistration(db, jobs.ToArray());
}