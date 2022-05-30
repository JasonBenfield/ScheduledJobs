using System;
using System.Collections.Generic;

namespace XTI_ScheduledJobSharedTests;

public sealed class DemoItemActionContext<T>
{
    private readonly List<string> values = new();

    public void ClearValues() => values.Clear();

    public string[] Values() => values.ToArray();

    public void AddValue(string value) => values.Add(value);

    private Func<DoSomethingItemData, bool> when = (_) => false;
    private string error = "";

    public void DontThrowError() => ThrowErrorWhen("", (_) => false);

    public void ThrowErrorWhen(string error, Func<DoSomethingItemData, bool> when)
    {
        this.error = error;
        this.when = when;
    }

    private JobTaskStatus errorStatus = JobTaskStatus.Values.Failed;

    public bool IsCanceledAfterError() => errorStatus.Equals(JobTaskStatus.Values.Canceled);

    public bool IsContinuedAfterError() => errorStatus.Equals(JobTaskStatus.Values.Completed);

    public bool IsRetryAfterError() => errorStatus.Equals(JobTaskStatus.Values.Retry);

    public void CancelAfterError() => errorStatus = JobTaskStatus.Values.Canceled;

    public void ContinueAfterError() => errorStatus = JobTaskStatus.Values.Completed;

    public TimeSpan RetryAfter { get; private set; } = TimeSpan.Zero;

    public void RetryAfterError(TimeSpan retryAfter)
    {
        this.RetryAfter = retryAfter;
        errorStatus = JobTaskStatus.Values.Retry;
    }

    public void MaybeThrowError(DoSomethingItemData data)
    {
        if (when(data))
        {
            throw new DemoItemActionException(error);
        }
    }
}
