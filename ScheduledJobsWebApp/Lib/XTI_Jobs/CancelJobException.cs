namespace XTI_Jobs;

public sealed class CancelJobException : Exception
{
    internal CancelJobException(string reason)
        : base($"Job cancelled: {reason}")
    {
        Reason = reason;
    }

    public string Reason { get; }
}
