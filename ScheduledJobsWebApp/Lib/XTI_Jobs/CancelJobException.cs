namespace XTI_Jobs;

public sealed class CancelJobException : Exception
{
    internal CancelJobException(string reason, DeletionTime deletionTime)
        : base($"Job cancelled\r\n{reason}\r\nDeletion Time: {deletionTime.DisplayText}")
    {
        Reason = reason;
        DeletionTime = deletionTime;
    }

    public string Reason { get; }
    public DeletionTime DeletionTime { get; }
}
