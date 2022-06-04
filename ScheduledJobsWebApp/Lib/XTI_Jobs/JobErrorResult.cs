namespace XTI_Jobs;

public sealed class JobErrorResult
{
    internal JobErrorResult(TriggeredJobTaskModel failedTask, JobTaskStatus updatedStatus, TimeSpan retryAfter, NextTaskModel[] nextTasks)
    {
        FailedTask = failedTask;
        UpdatedStatus = updatedStatus;
        RetryAfter = retryAfter;
        NextTasks = nextTasks;
    }

    public TriggeredJobTaskModel FailedTask { get; }
    public JobTaskStatus UpdatedStatus { get; }
    public TimeSpan RetryAfter { get; }
    public NextTaskModel[] NextTasks { get; }
}
