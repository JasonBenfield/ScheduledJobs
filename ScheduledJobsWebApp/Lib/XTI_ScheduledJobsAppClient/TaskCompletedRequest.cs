// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class TaskCompletedRequest
{
    public int JobID { get; set; }

    public int CompletedTaskID { get; set; }

    public NextTaskModel[] NextTasks { get; set; } = new NextTaskModel[0];
}