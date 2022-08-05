using XTI_Jobs.Abstractions;

namespace XTI_JobsDB.EF;

public sealed class ExpandedTriggeredJobEntity
{
    public int JobID { get; set; }
    public int EventNotificationID { get; set; }
    public int JobDefinitionID { get; set; }
    public string JobDisplayText { get; set; } = "";
    public int JobStatus { get; set; } = JobTaskStatus.Values.GetDefault().Value;
    public DateTimeOffset TimeJobStarted { get; set; }
    public DateTimeOffset TimeJobEnded { get; set; }
    public int TaskCount { get; set; }
}
