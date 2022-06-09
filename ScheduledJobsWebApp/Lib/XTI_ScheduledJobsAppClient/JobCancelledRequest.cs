// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class JobCancelledRequest
{
    public int TaskID { get; set; }

    public string Reason { get; set; } = "";
    public DeletionTime DeletionTime { get; set; } = DeletionTime.Values.GetDefault();
}