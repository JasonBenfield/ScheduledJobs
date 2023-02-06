namespace XTI_Jobs;

public sealed class JobRegistrationBuilder1
{
    private readonly JobRegistrationBuilder builder;
    private readonly JobKey jobKey;
    private TimeSpan timeout = TimeSpan.FromDays(1);
    private TimeSpan deleteAfter = TimeSpan.FromDays(365);
    private readonly List<JobRegistrationBuilder2> tasks = new();

    internal JobRegistrationBuilder1(JobRegistrationBuilder builder, JobKey jobKey)
    {
        this.builder = builder;
        this.jobKey = jobKey;
    }

    public JobRegistrationBuilder1 TimeoutAfter(TimeSpan timeout)
    {
        this.timeout = timeout;
        return this;
    }

    public JobRegistrationBuilder1 DeleteAfter(TimeSpan deleteAfter)
    {
        this.deleteAfter = deleteAfter;
        return this;
    }

    public JobRegistrationBuilder2 AddTask(JobTaskKey taskKey)
    {
        var taskBuilder = new JobRegistrationBuilder2(this, taskKey);
        tasks.Add(taskBuilder);
        return taskBuilder;
    }

    public JobRegistrationBuilder1 AddTasks(params JobTaskKey[] taskKeys)
    {
        foreach(var taskKey in taskKeys)
        {
            tasks.Add(new JobRegistrationBuilder2(this, taskKey));
        }
        return this;
    }

    public JobRegistrationBuilder1 AddTasks(JobTaskKey[] taskKeys, Action<JobTaskKey, JobRegistrationBuilder2> config)
    {
        foreach (var taskKey in taskKeys)
        {
            var builder2 = new JobRegistrationBuilder2(this, taskKey);
            config(taskKey, builder2);
            tasks.Add(builder2);
        }
        return this;
    }

    public JobRegistrationBuilder1 AddJob(JobKey jobKey) => builder.AddJob(jobKey);

    public JobRegistration Build() => builder.Build();

    internal RegisteredJob BuildJob() => 
        new RegisteredJob
        (
            jobKey, 
            timeout, 
            deleteAfter,
            tasks.Select(t => t.BuildTask()).ToArray()
        );
}