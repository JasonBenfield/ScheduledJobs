using XTI_Core;

namespace XTI_Jobs;

public sealed class TriggeredJobTask
{
    private readonly TriggeredJob job;

    internal TriggeredJobTask(TriggeredJob job, TriggeredJobTaskModel task)
    {
        this.job = job;
        Model = task;
    }

    public TriggeredJobTaskModel Model { get; }

    public JobTaskKey TaskKey { get => Model.TaskDefinition.TaskKey; }

    public T Data<T>() where T : new() => XtiSerializer.Deserialize<T>(Model.TaskData);

    public LogEntryModel[] Errors() =>
        Model.LogEntries
            .Where(e => e.Severity.Equals(AppEventSeverity.Values.CriticalError))
            .ToArray();

    public Task Failed(Exception ex) => job.TaskFailed(this, ex);

    public Task Completed(NextTaskModel[] nextTasks) => job.TaskCompleted(this, nextTasks);
}
