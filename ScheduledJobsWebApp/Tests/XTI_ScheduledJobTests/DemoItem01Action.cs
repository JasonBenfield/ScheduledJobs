﻿using System;

namespace XTI_ScheduledJobTests;

internal sealed class DemoItem01Action : JobAction<DoSomethingItemData>
{
    private readonly DemoItemActionContext<DemoItem01Action> context;

    public DemoItem01Action(TriggeredJobTask task, DemoItemActionContext<DemoItem01Action> context) : base(task)
    {
        this.context = context;
    }

    protected override Task<DoSomethingItemData> Execute(TriggeredJobTask task, JobActionResultBuilder nextTasks, DoSomethingItemData data)
    {
        context.MaybeThrowError(data);
        data.Value = $"Value{data.ItemID}";
        context.AddValue(data.Value);
        nextTasks.AddNext(DemoJobs.DoSomething.TaskItem02, data);
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