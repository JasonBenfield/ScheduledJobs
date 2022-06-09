namespace XTI_Jobs;

public sealed class CancelJobExceptionBuilder
{
    private string reason = "";
    private DeletionTime deletionTime = DeletionTime.Values.NextDay;

    public AfterBecause Because(string reason)
    {
        this.reason = reason;
        return new AfterBecause(this);
    }

    public AfterDeleteImmediately DeleteImmediately() => new AfterDeleteImmediately(this);

    private void Throw() => throw new CancelJobException(reason, deletionTime);

    public sealed class AfterBecause
    {
        private readonly CancelJobExceptionBuilder builder;

        public AfterBecause(CancelJobExceptionBuilder builder)
        {
            this.builder = builder;
        }

        public AfterBecause DeleteAtNormalTime()
        {
            builder.deletionTime = DeletionTime.Values.JobDefault;
            return this;
        }

        public void Throw() => builder.Throw();
    }

    public sealed class AfterDeleteImmediately
    {
        private readonly CancelJobExceptionBuilder builder;

        public AfterDeleteImmediately(CancelJobExceptionBuilder builder)
        {
            this.builder = builder;
            builder.deletionTime = DeletionTime.Values.Immediately;
        }

        public void Throw() => builder.Throw();
    }
}

