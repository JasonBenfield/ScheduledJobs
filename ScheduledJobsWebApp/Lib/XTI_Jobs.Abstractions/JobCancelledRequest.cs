namespace XTI_Jobs.Abstractions;

public sealed class JobCancelledRequest
{
    public JobCancelledRequest()
        : this(0, "")
    {
    }

    public JobCancelledRequest(int taskID, string reason)
    {
        TaskID = taskID;
        Reason = reason;
    }

    public int TaskID { get; set; }
    public string Reason { get; set; }
}
