namespace XTI_ScheduledJobTests;

internal sealed class DemoAction01 : JobAction<DoSomethingData>
{
    private readonly DemoActionContext<DemoAction01> context;

    public DemoAction01(TriggeredJobTask task, DemoActionContext<DemoAction01> context)
        : base(task)
    {
        this.context = context;
    }

    protected override Task<DoSomethingData> Execute(TriggeredJobTask task, DoSomethingData data)
    {
        context.NumberOfTimesExecuted++;
        data.Output += "Action1";
        context.TargetID = data.TargetID;
        context.Output = data.Output;
        return Task.FromResult(data);
    }

    protected override NextTask[] Next(TriggeredJobTask task, DoSomethingData data) =>
        new[] { new NextTask(DemoJobs.DoSomething.Task02, data) };
}
