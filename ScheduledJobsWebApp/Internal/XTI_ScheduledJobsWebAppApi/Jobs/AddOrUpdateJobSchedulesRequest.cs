namespace XTI_ScheduledJobsWebAppApi.Jobs;

public sealed class AddOrUpdateJobSchedulesRequest
{
    public string JobKey { get; set; } = "";
    public string Schedules { get; set; } = "";
    public TimeSpan DeleteAfter { get; set; }
}
