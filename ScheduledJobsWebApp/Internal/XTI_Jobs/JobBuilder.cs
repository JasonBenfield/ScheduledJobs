namespace XTI_Jobs;

public sealed class JobBuilder
{
    private readonly JobKey jobKey;
    private readonly List<JobTaskKey> taskKeys = new();

    internal JobBuilder(JobKey jobKey)
    {
        this.jobKey = jobKey;
    }

    public JobTaskBuilder AddFirstTask(JobTaskKey taskKey)
    {
        taskKeys.Add(taskKey);
        return new JobTaskBuilder(taskKeys);
    }

    internal RegisteredJob Build() => new RegisteredJob(jobKey, taskKeys.ToArray());
}

public sealed class JobTaskBuilder
{
    private readonly List<JobTaskKey> taskKeys;

    internal JobTaskBuilder(List<JobTaskKey> taskKeys)
    {
        this.taskKeys = taskKeys;
    }

    public JobTaskBuilder AddTask(JobTaskKey taskKey)
    {
        taskKeys.Add(taskKey);
        return this;
    }

}
