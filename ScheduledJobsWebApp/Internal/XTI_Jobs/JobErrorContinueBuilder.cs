namespace XTI_Jobs;

public sealed class JobErrorContinueBuilder
{
    private readonly JobErrorResultBuilder builder;

    internal JobErrorContinueBuilder(JobErrorResultBuilder builder)
    {
        this.builder = builder;
    }

    public JobErrorContinueBuilder AddNext(JobTaskKey taskKey, object[] items)
    {
        foreach (var item in items)
        {
            AddNext(taskKey, item);
        }
        return this;
    }

    public JobErrorContinueBuilder AddNext(JobTaskKey taskKey, object? data = null)
    {
        builder.AddNext(taskKey, data);
        return this;
    }
}
