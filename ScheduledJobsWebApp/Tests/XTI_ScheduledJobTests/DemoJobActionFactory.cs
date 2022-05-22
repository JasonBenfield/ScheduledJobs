using System;

namespace XTI_ScheduledJobTests;

internal sealed class DemoJobActionFactory : IJobActionFactory
{
    public DemoJobActionFactory()
    {
        Action01Context = new DemoActionContext<DemoAction01>();
        Action02Context = new DemoActionContext<DemoAction02>();
        ItemAction01Context = new DemoItemActionContext<DemoItem01Action>();
        ItemAction02Context = new DemoItemActionContext<DemoItem02Action>();
    }

    public DemoActionContext<DemoAction01> Action01Context { get; }
    public DemoActionContext<DemoAction02> Action02Context { get; }
    public DemoItemActionContext<DemoItem01Action> ItemAction01Context { get; }
    public DemoItemActionContext<DemoItem02Action> ItemAction02Context { get; }

    public IJobAction CreateJobAction(TriggeredJobTask jobTask)
    {
        IJobAction action;
        if (jobTask.TaskKey.Equals(DemoJobs.DoSomething.Task01))
        {
            action = new DemoAction01(jobTask, Action01Context);
        }
        else if (jobTask.TaskKey.Equals(DemoJobs.DoSomething.Task02))
        {
            action = new DemoAction02(jobTask, Action02Context);
        }
        else if (jobTask.TaskKey.Equals(DemoJobs.DoSomething.TaskItem01))
        {
            action = new DemoItem01Action(jobTask, ItemAction01Context);
        }
        else if (jobTask.TaskKey.Equals(DemoJobs.DoSomething.TaskItem02))
        {
            action = new DemoItem02Action(jobTask, ItemAction02Context);
        }
        else
        {
            throw new NotSupportedException($"Task '{jobTask.TaskKey.DisplayText}' is not supported");
        }
        return action;
    }

    public ITransformedSourceData CreateTransformedSourceData(string sourceData) => new DemoTransformedSourceData(sourceData);

    public NextTaskModel[] FirstTasks(string taskData) => 
        new[] 
        { 
            new NextTaskModel(DemoJobs.DoSomething.Task01, taskData)
        };
}
