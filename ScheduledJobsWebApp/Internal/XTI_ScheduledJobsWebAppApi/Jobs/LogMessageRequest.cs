namespace XTI_ScheduledJobsWebAppApi.Jobs;

public sealed class LogMessageRequest
{
    public int TaskID { get; set; }
    public string Category { get; set; } = "";
    public string Message { get; set; } = "";
    public string Details { get; set; } = "";
}
