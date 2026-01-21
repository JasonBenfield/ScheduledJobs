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

    public JobLogEntryModel[] Errors() =>
        Model.LogEntries
            .Where(e => e.Severity.Equals(AppEventSeverity.Values.CriticalError))
            .ToArray();

    public JobLogEntryModel[] Messages() =>
        Model.LogEntries
            .Where(e => !e.Severity.Equals(AppEventSeverity.Values.CriticalError))
            .ToArray();

    public Task LogMessage(string message, CancellationToken ct) => LogMessage("", message, "", ct);

    public Task LogMessage(string category, string message, string details, CancellationToken ct) =>
        job.LogMessage(this, category, message, details, ct);

    internal Task<TriggeredJobTask?> Failed
    (
        JobTaskStatus errorStatus,
        TimeSpan retryAfter,
        NextTaskModel[] nextTasks,
        Exception ex,
        CancellationToken ct
    ) => job.TaskFailed(this, errorStatus, retryAfter, nextTasks, ex, ct);

    internal Task Completed(bool preserveData, NextTaskModel[] nextTasks, CancellationToken ct) =>
        job.TaskCompleted(this, preserveData, nextTasks, ct);

    internal Task CancelJob(string reason, CancellationToken ct) => job.CancelJob(this, reason, ct);
}
