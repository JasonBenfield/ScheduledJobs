namespace XTI_Jobs;

public interface IJobActionFactory
{
    ITransformedSourceData CreateTransformedSourceData(string sourceData);
    NextTaskModel[] FirstTasks(string taskData);
    IJobAction CreateJobAction(TriggeredJobTask jobTask);
}
