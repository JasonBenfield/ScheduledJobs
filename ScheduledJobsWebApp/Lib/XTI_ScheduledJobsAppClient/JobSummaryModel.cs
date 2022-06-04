// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class JobSummaryModel
{
    public int ID { get; set; }

    public JobKey JobKey { get; set; } = new JobKey();
    public JobTaskStatus Status { get; set; } = JobTaskStatus.Values.GetDefault();
    public DateTimeOffset TimeStarted { get; set; }

    public DateTimeOffset TimeEnded { get; set; }

    public int TaskCount { get; set; }
}