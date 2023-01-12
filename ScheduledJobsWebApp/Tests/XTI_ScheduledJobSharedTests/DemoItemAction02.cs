namespace XTI_ScheduledJobSharedTests;

public sealed class DemoItemAction02 : JobAction<DoSomethingItemData>
{
    private readonly DemoItemActionContext<DemoItemAction02> context;

    public DemoItemAction02(TriggeredJobTask task, DemoItemActionContext<DemoItemAction02> context) : base(task)
    {
        this.context = context;
    }

    protected override Task Execute(CancellationToken stoppingToken, TriggeredJobTask task, JobActionResultBuilder nextTasks, DoSomethingItemData data)
    {
        context.MaybeThrowError(data);
        context.MaybeCancel(data, () => CancelJob());
        data.Value = $"Value{data.ItemID}";
        context.AddValue(data.Value);
        return Task.CompletedTask;
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
