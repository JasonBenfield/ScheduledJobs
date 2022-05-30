namespace XTI_ScheduledJobsWebAppApi.Jobs;

public sealed class TriggerJobsRequest
{
    public EventKey EventKey { get; set; } = new EventKey("");
    public JobKey JobKey { get; set; } = new JobKey("");
}
