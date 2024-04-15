namespace XTI_Jobs.Abstractions;

public sealed class TaskCompletedRequest
{
    public TaskCompletedRequest()
        : this(0, false, new NextTaskModel[0])
    {
    }

    public TaskCompletedRequest(int completedTaskID, bool preserveData, params NextTaskModel[] nextTasks)
    {
        CompletedTaskID = completedTaskID;
        PreserveData = preserveData;
        NextTasks = nextTasks;
    }

    public int CompletedTaskID { get; set; }
    public bool PreserveData { get; set; }
    public NextTaskModel[] NextTasks { get; set; }
}
