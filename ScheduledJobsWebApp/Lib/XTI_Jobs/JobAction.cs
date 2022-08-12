namespace XTI_Jobs;

public abstract class JobAction<T> : IJobAction
    where T : new()
{
    private readonly TriggeredJobTask task;
    private T data = new T();
    private bool isCancelled = false;

    protected JobAction(TriggeredJobTask task)
    {
        this.task = task;
    }

    public async Task<JobActionResult> Execute(CancellationToken stoppingToken)
    {
        data = task.Data<T>();
        var resultBuilder = new JobActionResultBuilder();
        await Execute(stoppingToken, task, resultBuilder, data);
        if (isCancelled)
        {
            throw new CancelJobException("");
        }
        return resultBuilder.Build();
    }

    protected abstract Task Execute(CancellationToken stoppingToken, TriggeredJobTask task, JobActionResultBuilder next, T data);

    protected CancelJobExceptionBuilder CancelJob()
    {
        isCancelled = true;
        return new CancelJobExceptionBuilder();
    }

    public async Task<JobErrorResult> OnError(Exception ex)
    {
        var resultBuilder = new JobErrorResultBuilder(task.Model);
        await OnError(ex, data, resultBuilder);
        return resultBuilder.Build();
    }

    protected virtual Task OnError(Exception ex, T data, JobErrorResultBuilder onError) => Task.CompletedTask;
}
