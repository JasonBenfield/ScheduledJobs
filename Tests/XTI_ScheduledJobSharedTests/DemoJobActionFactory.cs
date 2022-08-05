using System;

namespace XTI_ScheduledJobSharedTests;

public sealed class DemoJobActionFactory : IJobActionFactory
{
    public DemoJobActionFactory()
    {
        Action01Context = new DemoActionContext<DemoAction01>();
        Action02Context = new DemoActionContext<DemoAction02>();
        ItemAction01Context = new DemoItemActionContext<DemoItemAction01>();
        ItemAction02Context = new DemoItemActionContext<DemoItemAction02>();
        ActionFinalContext = new DemoActionContext<DemoActionFinal>();
    }

    public DemoActionContext<DemoAction01> Action01Context { get; }
    public DemoActionContext<DemoAction02> Action02Context { get; }
    public DemoItemActionContext<DemoItemAction01> ItemAction01Context { get; }
    public DemoItemActionContext<DemoItemAction02> ItemAction02Context { get; }
    public DemoActionContext<DemoActionFinal> ActionFinalContext { get; }

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
            action = new DemoItemAction01(jobTask, ItemAction01Context);
        }
        else if (jobTask.TaskKey.Equals(DemoJobs.DoSomething.TaskItem02))
        {
            action = new DemoItemAction02(jobTask, ItemAction02Context);
        }
        else if (jobTask.TaskKey.Equals(DemoJobs.DoSomething.TaskFinal))
        {
            action = new DemoActionFinal(jobTask, ActionFinalContext);
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
