namespace XTI_Jobs;

public sealed class JobRegistrationBuilder2
{
    private readonly JobRegistrationBuilder1 builder;
    private readonly JobTaskKey taskKey;
    private TimeSpan timeout = TimeSpan.FromHours(1);

    public JobRegistrationBuilder2(JobRegistrationBuilder1 builder, JobTaskKey taskKey)
    {
        this.builder = builder;
        this.taskKey = taskKey;
    }

    public JobRegistrationBuilder2 TimeoutAfter(TimeSpan timeout)
    {
        this.timeout = timeout;
        return this;
    }

    public JobRegistrationBuilder2 AddTask(JobTaskKey taskKey) => builder.AddTask(taskKey);

    public JobRegistrationBuilder1 AddJob(JobKey jobKey) => builder.AddJob(jobKey);

    public JobRegistration Build() => builder.Build();

    internal RegisteredJobTask BuildTask() => new RegisteredJobTask(taskKey, timeout);
}
