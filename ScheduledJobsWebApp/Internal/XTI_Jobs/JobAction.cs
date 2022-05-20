namespace XTI_Jobs;

public abstract class JobAction<T> : IJobAction
    where T : new()
{
    private readonly TriggeredJobTask task;

    protected JobAction(TriggeredJobTask task)
    {
        this.task = task;
    }

    public async Task<JobActionResult> Execute()
    {
        var data = task.Data<T>();
        data = await Execute(task, data);
        var nextTasks = Next(task, data);
        return new JobActionResult(task.Model, nextTasks.Select(nt => nt.ToModel()).ToArray());
    }

    protected abstract Task<T> Execute(TriggeredJobTask task, T data);

    protected abstract NextTask[] Next(TriggeredJobTask task, T data);
}
