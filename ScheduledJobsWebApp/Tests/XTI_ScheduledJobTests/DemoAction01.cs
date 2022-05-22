using System;

namespace XTI_ScheduledJobTests;

internal sealed class DemoAction01 : JobAction<DoSomethingData>
{
    private readonly DemoActionContext<DemoAction01> context;

    public DemoAction01(TriggeredJobTask task, DemoActionContext<DemoAction01> context)
        : base(task)
    {
        this.context = context;
    }

    protected override async Task<DoSomethingData> Execute(TriggeredJobTask task, JobActionResultBuilder nextTasks, DoSomethingData data)
    {
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
