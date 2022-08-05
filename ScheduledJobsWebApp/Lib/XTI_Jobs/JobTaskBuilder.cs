namespace XTI_Jobs;

public sealed class JobTaskBuilder
{
    private readonly JobBuilder builder;
    private readonly JobTaskKey taskKey;
    private TimeSpan timeout = TimeSpan.FromHours(1);

    public JobTaskBuilder(JobBuilder builder, JobTaskKey taskKey)
    {
        this.builder = builder;
        this.taskKey = taskKey;
    }

    public JobTaskBuilder TimeoutAfter(TimeSpan timeout)
    {
        this.timeout = timeout;
        return this;
    }

    public JobTaskBuilder AddTask(JobTaskKey taskKey) => builder.AddTask(taskKey);

    internal RegisteredJobTask Build() => new RegisteredJobTask(taskKey, timeout);
}
