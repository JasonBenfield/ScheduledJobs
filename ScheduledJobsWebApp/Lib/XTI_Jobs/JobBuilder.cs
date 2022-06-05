﻿namespace XTI_Jobs;

public sealed class JobBuilder
{
    private readonly JobKey jobKey;
    private TimeSpan timeout;
    private TimeSpan deleteAfter = TimeSpan.FromDays(365);
    private readonly List<JobTaskBuilder> tasks = new();

    internal JobBuilder(JobKey jobKey)
    {
        this.jobKey = jobKey;
    }

    public JobBuilder TimeoutAfter(TimeSpan timeout)
    {
        this.timeout = timeout;
        return this;
    }

    public JobBuilder DeleteAfter(TimeSpan deleteAfter)
    {
        this.deleteAfter = deleteAfter;
        return this;
    }

    public JobTaskBuilder AddTask(JobTaskKey taskKey)
    {
        var taskBuilder = new JobTaskBuilder(this, taskKey);
        tasks.Add(taskBuilder);
        return taskBuilder;
    }

    internal RegisteredJob Build() => 
        new RegisteredJob
        (
            jobKey, 
            timeout, 
            deleteAfter,
            tasks.Select(t => t.Build()).ToArray()
        );
}