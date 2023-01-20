namespace XTI_Jobs;

public interface IJobActionFactory
{
    NextTaskModel[] FirstTasks(string taskData);
    IJobAction CreateJobAction(TriggeredJobTask jobTask);
}
