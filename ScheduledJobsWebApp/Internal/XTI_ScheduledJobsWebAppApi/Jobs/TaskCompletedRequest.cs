namespace XTI_ScheduledJobsWebAppApi.Jobs;

public sealed class TaskCompletedRequest
{
    public int JobID { get; set; }
    public int CompletedTaskID { get; set; }
    public bool PreserveData { get; set; }
    public NextTaskModel[] NextTasks { get; set; } = new NextTaskModel[0];
}
