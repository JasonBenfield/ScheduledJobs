namespace XTI_ScheduledJobsWebAppApi.Jobs;

public sealed class StartJobRequest
{
    public int JobID { get; set; }
    public NextTaskModel[] NextTasks { get; set; } = new NextTaskModel[0];
}
