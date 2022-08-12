namespace XTI_Jobs;

public interface IJobActionFactory
{
    Task<string> TransformSourceData(string sourceKey, string sourceData);
    NextTaskModel[] FirstTasks(string taskData);
    IJobAction CreateJobAction(TriggeredJobTask jobTask);
}
