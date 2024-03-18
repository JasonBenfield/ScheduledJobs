namespace XTI_Jobs.Abstractions;

public sealed class GetTaskRequest
{
    public GetTaskRequest()
        : this(0)
    {
    }

    public GetTaskRequest(int taskID)
    {
        TaskID = taskID;
    }

    public int TaskID { get; set; }
}
