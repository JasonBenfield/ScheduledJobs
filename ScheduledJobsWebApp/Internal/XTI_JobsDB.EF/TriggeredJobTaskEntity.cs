using XTI_Jobs.Abstractions;

namespace XTI_JobsDB.EF;

public sealed class TriggeredJobTaskEntity
{
    public int ID { get; set; }
    public int TriggeredJobID { get; set; }
    public int TaskDefinitionID { get; set; }
    public DateTimeOffset TimeAdded { get; set; } = DateTimeOffset.MaxValue;
    public DateTimeOffset TimeStarted { get; set; } = DateTimeOffset.MaxValue;
    public DateTimeOffset TimeEnded { get; set; } = DateTimeOffset.MaxValue;
    public int Status { get; set; } = JobTaskStatus.Values.NotRun;
    public string TaskData { get; set; } = "";
}
