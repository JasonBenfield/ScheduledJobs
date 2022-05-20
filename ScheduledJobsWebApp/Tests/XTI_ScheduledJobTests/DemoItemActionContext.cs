using System;
using System.Collections.Generic;

namespace XTI_ScheduledJobTests;

internal sealed class DemoItemActionContext<T>
{
    private readonly List<string> values = new();

    public string[] Values() => values.ToArray();

    public void AddValue(string value) => values.Add(value);

    private Func<DoSomethingItemData, bool> when = (_) => false;
    private string error = "";

    public void ThrowErrorWhen(string error, Func<DoSomethingItemData, bool> when)
    {
        this.error = error;
        this.when = when;
    }

    public void MaybeThrowError(DoSomethingItemData data)
    {
        if (when(data))
        {
            throw new DemoItemActionException(error);
        }
    }
}
