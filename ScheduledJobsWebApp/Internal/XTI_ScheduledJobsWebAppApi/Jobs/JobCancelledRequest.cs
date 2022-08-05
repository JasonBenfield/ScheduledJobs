namespace XTI_ScheduledJobsWebAppApi.Jobs;

public sealed class JobCancelledRequest
{
    public int TaskID { get; set; }
    public string Reason { get; set; } = "";
}
