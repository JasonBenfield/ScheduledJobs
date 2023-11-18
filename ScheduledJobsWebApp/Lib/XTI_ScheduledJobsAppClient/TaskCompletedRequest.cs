// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class TaskCompletedRequest
{
    public int CompletedTaskID { get; set; }
    public bool PreserveData { get; set; }
    public NextTaskModel[] NextTasks { get; set; } = new NextTaskModel[0];
}