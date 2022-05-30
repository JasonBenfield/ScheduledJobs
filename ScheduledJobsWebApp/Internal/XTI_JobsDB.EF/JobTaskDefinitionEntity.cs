namespace XTI_JobsDB.EF;

public sealed class JobTaskDefinitionEntity
{
    public int ID { get; set; }
    public int JobDefinitionID { get; set; }
    public string TaskKey { get; set; } = "";
    public TimeSpan Timeout { get; set; } = TimeSpan.Zero;
}
