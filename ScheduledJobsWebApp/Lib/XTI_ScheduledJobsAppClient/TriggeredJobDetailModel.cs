// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class TriggeredJobDetailModel
{
    public TriggeredJobModel Job { get; set; } = new TriggeredJobModel();
    public EventNotificationModel TriggeredBy { get; set; } = new EventNotificationModel();
    public TriggeredJobTaskModel[] Tasks { get; set; } = new TriggeredJobTaskModel[0];
}