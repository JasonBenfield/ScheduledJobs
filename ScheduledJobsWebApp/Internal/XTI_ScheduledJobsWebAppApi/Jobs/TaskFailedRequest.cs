namespace XTI_ScheduledJobsWebAppApi.Jobs;

public sealed class TaskFailedRequest
{
    public int FailedTaskID { get; set; }
    public JobTaskStatus ErrorStatus { get; set; } = JobTaskStatus.Values.NotSet;
    public TimeSpan RetryAfter { get; set; }
    public NextTaskModel[] NextTasks { get; set; } = new NextTaskModel[0];
    public string Category { get; set; } = "";
    public string Message { get; set; } = "";
    public string Detail { get; set; } = "";
    public string SourceLogEntryKey { get; set; } = "";
}
