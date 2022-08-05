namespace XTI_Jobs;

public sealed class CancelJobExceptionBuilder
{
    private string reason = "";

    public CancelJobExceptionBuilder Because(string reason)
    {
        this.reason = reason;
        return this;
    }

    public void Throw() => throw new CancelJobException(reason);

}

