﻿using System;

namespace XTI_ScheduledJobSharedTests;

public sealed class DemoAction02 : JobAction<DoSomethingData>
{
    private readonly DemoActionContext<DemoAction02> context;

    public DemoAction02(TriggeredJobTask task, DemoActionContext<DemoAction02> context)
        : base(task)
    {
        this.context = context;
    }

    protected override async Task Execute(CancellationToken stoppingToken, TriggeredJobTask task, JobActionResultBuilder nextTasks, DoSomethingData data)
    {
        if (!context.Delay.Equals(TimeSpan.Zero))
        {
            await Task.Delay(context.Delay, stoppingToken);
        }
        context.NumberOfTimesExecuted++;
        data.Output += ",Action2";
        context.TargetID = data.TargetID;
        context.Output = data.Output;
        foreach (var message in context.Messages)
        {
            await task.LogMessage(message);
        }
        nextTasks.AddNext(DemoJobs.DoSomething.TaskItem01, data.Items);
    }
}
