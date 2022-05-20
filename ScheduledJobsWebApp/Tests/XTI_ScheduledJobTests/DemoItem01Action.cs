using System;

namespace XTI_ScheduledJobTests;

internal sealed class DemoItem01Action : JobAction<DoSomethingItemData>
{
    private readonly DemoItemActionContext<DemoItem01Action> context;

    public DemoItem01Action(TriggeredJobTask task, DemoItemActionContext<DemoItem01Action> context) : base(task)
    {
        this.context = context;
    }

    protected override Task<DoSomethingItemData> Execute(TriggeredJobTask task, DoSomethingItemData data)
    {
        context.MaybeThrowError(data);
        data.Value = $"Value{data.ItemID}";
        context.AddValue(data.Value);
        return Task.FromResult(data);
    }

    protected override NextTask[] Next(TriggeredJobTask task, DoSomethingItemData data)
    {
        return new NextTask[0]; 
    }
}
