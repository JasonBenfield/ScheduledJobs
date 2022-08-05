namespace XTI_ScheduledJobsWebAppApi.Jobs;

public sealed class RetryJobsRequest
{
    public JobKey JobKey { get; set; } = new JobKey("");
}
