namespace XTI_ScheduledJobsWebAppApi.Tasks;

public sealed class EditTaskDataRequest
{
    public int TaskID { get; set; }
    public string TaskData { get; set; } = "";
}
