namespace XTI_JobsDB.EF;

public sealed class JobScheduleEntity
{
    public int ID { get; set; }
    public int JobDefinitionID { get; set; }
    public string Serialized { get; set; } = "";
}
