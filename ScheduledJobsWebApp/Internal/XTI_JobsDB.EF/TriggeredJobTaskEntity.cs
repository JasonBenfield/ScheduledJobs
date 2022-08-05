using XTI_Jobs.Abstractions;

namespace XTI_JobsDB.EF;

public sealed class TriggeredJobTaskEntity
{
    public int ID { get; set; }
    public int TriggeredJobID { get; set; }
    public int TaskDefinitionID { get; set; }
    public int Generation { get; set; }
    public int Sequence { get; set; }
    public DateTimeOffset TimeAdded { get; set; } = DateTimeOffset.MaxValue;
    public DateTimeOffset TimeActive { get; set; } = DateTimeOffset.MaxValue;
    public DateTimeOffset TimeInactive { get; set; } = DateTimeOffset.MaxValue;
    public DateTimeOffset TimeStarted { get; set; } = DateTimeOffset.MaxValue;
    public DateTimeOffset TimeEnded { get; set; } = DateTimeOffset.MaxValue;
    public int Status { get; set; } = JobTaskStatus.Values.NotSet;
    public string TaskData { get; set; } = "";
}
