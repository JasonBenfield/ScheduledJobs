namespace XTI_ScheduledJobTests;

internal sealed class DemoAction02 : JobAction<DoSomethingData>
{
    private readonly DemoActionContext<DemoAction02> context;

    public DemoAction02(TriggeredJobTask task, DemoActionContext<DemoAction02> context)
        : base(task)
    {
        this.context = context;
    }

    protected override async Task<DoSomethingData> Execute(TriggeredJobTask task, JobActionResultBuilder nextTasks, DoSomethingData data)
    {
        context.NumberOfTimesExecuted++;
        data.Output += ",Action2";
        context.TargetID = data.TargetID;
        context.Output = data.Output;
        foreach (var message in context.Messages)
        {
            await task.LogMessage(message);
        }
        nextTasks.AddNext(DemoJobs.DoSomething.TaskItem01, data.Items);
        return data;
    }
}
