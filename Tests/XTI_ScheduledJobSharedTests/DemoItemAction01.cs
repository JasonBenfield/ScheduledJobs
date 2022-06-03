using System;

namespace XTI_ScheduledJobSharedTests;

public sealed class DemoItemAction01 : JobAction<DoSomethingItemData>
{
    private readonly DemoItemActionContext<DemoItemAction01> context;

    public DemoItemAction01(TriggeredJobTask task, DemoItemActionContext<DemoItemAction01> context) : base(task)
    {
        this.context = context;
    }

    protected override Task<DoSomethingItemData> Execute(CancellationToken stoppingToken, TriggeredJobTask task, JobActionResultBuilder next, DoSomethingItemData data)
    {
        context.MaybeThrowError(data);
        data.Value = $"Value{data.ItemID}";
        context.AddValue(data.Value);
        next
            .PreserveData()
            .AddNext(DemoJobs.DoSomething.TaskItem02, data);
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
            onError.Continue()
                .AddNext(DemoJobs.DoSomething.TaskItem02, data);
        }
        else if (context.IsRetryAfterError())
        {
            onError.Retry().After(context.RetryAfter);
        }
        return Task.CompletedTask;
    }
}
