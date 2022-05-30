using System;

namespace XTI_ScheduledJobSharedTests;

public sealed class DemoAction01 : JobAction<DoSomethingData>
{
    private readonly DemoActionContext<DemoAction01> context;

    public DemoAction01(TriggeredJobTask task, DemoActionContext<DemoAction01> context)
        : base(task)
    {
        this.context = context;
    }

    protected override async Task<DoSomethingData> Execute(CancellationToken stoppingToken, TriggeredJobTask task, JobActionResultBuilder nextTasks, DoSomethingData data)
    {
        if (!context.Delay.Equals(TimeSpan.Zero))
        {
            await Task.Delay(context.Delay, stoppingToken);
        }
        context.NumberOfTimesExecuted++;
        data.Output += "Action1";
        context.TargetID = data.TargetID;
        context.Output = data.Output;
        foreach(var message in context.Messages)
        {
            await task.LogMessage(message);
        }
        nextTasks.AddNext(DemoJobs.DoSomething.Task02, data);
        return data;
    }
}
