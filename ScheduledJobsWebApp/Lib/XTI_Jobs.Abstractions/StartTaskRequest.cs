namespace XTI_Jobs.Abstractions;

public sealed class StartTaskRequest
{
    public StartTaskRequest()
        : this(0)
    {
    }

    public StartTaskRequest(int taskID)
    {
        TaskID = taskID;
    }

    public int TaskID { get; set; }
}
