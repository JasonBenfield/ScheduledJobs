namespace XTI_JobsDB.Entities;

public sealed class EventDefinitionEntity
{
    public int ID { get; set; }
    public string EventKey { get; set; } = "";
    public bool CompareSourceKeyAndDataForDuplication { get; set; }
    public int DuplicateHandling { get; set; }
}
