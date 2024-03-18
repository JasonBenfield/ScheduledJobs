namespace XTI_Jobs.Abstractions;

public sealed class TaskFailedRequest
{
    public TaskFailedRequest()
        : this(0, JobTaskStatus.Values.NotSet, new TimeSpan(), "", "", "", "", new NextTaskModel[0])
    {
    }

    public TaskFailedRequest
    (
        int failedTaskID,
        JobTaskStatus errorStatus,
        TimeSpan retryAfter,
        string category,
        string message,
        string detail,
        string sourceLogEntryKey,
        params NextTaskModel[] nextTasks
    )
    {
        FailedTaskID = failedTaskID;
        ErrorStatus = errorStatus.Value;
        RetryAfter = retryAfter;
        NextTasks = nextTasks;
        Category = category;
        Message = message;
        Detail = detail;
        SourceLogEntryKey = sourceLogEntryKey;
    }

    public int FailedTaskID { get; set; }
    public int ErrorStatus { get; set; }
    public TimeSpan RetryAfter { get; set; }
    public NextTaskModel[] NextTasks { get; set; } = new NextTaskModel[0];
    public string Category { get; set; } = "";
    public string Message { get; set; } = "";
    public string Detail { get; set; } = "";
    public string SourceLogEntryKey { get; set; } = "";
}
