using System;

namespace XTI_ScheduledJobTests;

internal sealed class DemoItem02Action : JobAction<DoSomethingItemData>
{
    private readonly DemoItemActionContext<DemoItem02Action> context;

    public DemoItem02Action(TriggeredJobTask task, DemoItemActionContext<DemoItem02Action> context) : base(task)
    {
        this.context = context;
    }

    protected override Task<DoSomethingItemData> Execute(TriggeredJobTask task, JobActionResultBuilder nextTasks, DoSomethingItemData data)
    {
        context.MaybeThrowError(data);
        data.Value = $"Value{data.ItemID}";
        context.AddValue(data.Value);
        return Task.FromResult(data);
    }

    protected override Task OnError(Exception ex, DoSomethingItemData data, JobErrorResultBuilder onError)
    {
        if (context.IsCanceledAfterError())
        {
            onError.Cancel();
        }
        else if (context.IsContinuedAfterError())
        {
            onError.Continue();
        }
        else if (context.IsRetryAfterError())
        {
            onError.Retry().After(context.RetryAfter);
        }
        return Task.CompletedTask;
    }
}
