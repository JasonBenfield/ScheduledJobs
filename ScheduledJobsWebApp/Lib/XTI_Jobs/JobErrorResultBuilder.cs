using XTI_Core;

namespace XTI_Jobs;

public sealed class JobErrorResultBuilder
{
    private readonly TriggeredJobTaskModel task;
    private JobTaskStatus errorStatus = JobTaskStatus.Values.Failed;
    private readonly List<NextTaskModel> nextTasks = new();
    private TimeSpan retryAfter = TimeSpan.FromHours(1);

    public JobErrorResultBuilder(TriggeredJobTaskModel task)
    {
        this.task = task;
    }

    public JobErrorResultBuilder Cancel()
    {
        errorStatus = JobTaskStatus.Values.Canceled;
        return this;
    }

    public JobErrorContinueBuilder Continue()
    {
        errorStatus = JobTaskStatus.Values.Completed;
        return new JobErrorContinueBuilder(this);
    }

    public JobErrorRetryBuilder Retry()
    {
        errorStatus = JobTaskStatus.Values.Retry;
        return new JobErrorRetryBuilder(this);
    }

    internal void AddNext(JobTaskKey taskKey, object? data)
    {
        var serialized = XtiSerializer.Serialize(data ?? new object());
        var nextTask = new NextTaskModel(taskKey, serialized);
        nextTasks.Add(nextTask);
    }

    internal void RetryAfter(TimeSpan retryAfter) => this.retryAfter = retryAfter;

    internal JobErrorResult Build() => new JobErrorResult(task, errorStatus, retryAfter, nextTasks.ToArray());
}

public sealed class JobErrorRetryBuilder
{
    private readonly JobErrorResultBuilder builder;

    internal JobErrorRetryBuilder(JobErrorResultBuilder builder)
    {
        this.builder = builder;
    }

    public JobErrorRetryBuilder After(TimeSpan retryAfter)
    {
        builder.RetryAfter(retryAfter);
        return this;
    }
}
