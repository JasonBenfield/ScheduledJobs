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

    public T Data<T>() where T : new() => 
        string.IsNullOrWhiteSpace(Model.TaskData) 
            ? new T() 
            : XtiSerializer.Deserialize<T>(Model.TaskData);

    public LogEntryModel[] Errors() =>
        Model.LogEntries
            .Where(e => e.Severity.Equals(AppEventSeverity.Values.CriticalError))
            .ToArray();

    public LogEntryModel[] Messages() =>
        Model.LogEntries
            .Where(e => !e.Severity.Equals(AppEventSeverity.Values.CriticalError))
            .ToArray();

    public Task LogMessage(string message) => LogMessage("", message, "");

    public Task LogMessage(string category, string message, string details) =>
        job.LogMessage(this, category, message, details);

    internal Task<TriggeredJobTask?> Failed
    (
        JobTaskStatus errorStatus, 
        TimeSpan retryAfter, 
        NextTaskModel[] nextTasks, 
        Exception ex
    ) => job.TaskFailed(this, errorStatus, retryAfter, nextTasks, ex);

    internal Task Completed(bool preserveData, NextTaskModel[] nextTasks) => 
        job.TaskCompleted(this, preserveData, nextTasks);

    internal Task CancelJob(string reason, DeletionTime deletionTime) =>
        job.CancelJob(this, reason, deletionTime);
}
