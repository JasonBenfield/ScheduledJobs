using System.Linq;

namespace XTI_ScheduledJobTests;

internal sealed class DemoAction02 : JobAction<DoSomethingData>
{
    private readonly DemoActionContext<DemoAction02> context;

    public DemoAction02(TriggeredJobTask task, DemoActionContext<DemoAction02> context)
        : base(task)
    {
        this.context = context;
    }

    protected override Task<DoSomethingData> Execute(TriggeredJobTask task, DoSomethingData data)
    {
        context.NumberOfTimesExecuted++;
        data.Output += ",Action2";
        context.TargetID = data.TargetID;
        context.Output = data.Output;
        return Task.FromResult(data);
    }

    protected override NextTask[] Next(TriggeredJobTask task, DoSomethingData data) => 
        data.Items
            .Select(item => new NextTask(DemoJobs.DoSomething.TaskItem, item))
            .ToArray();
}
