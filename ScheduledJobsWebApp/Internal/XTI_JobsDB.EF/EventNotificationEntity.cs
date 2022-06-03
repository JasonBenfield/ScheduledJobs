namespace XTI_JobsDB.Entities;

public sealed class EventNotificationEntity
{
    public int ID { get; set; }
    public int EventDefinitionID { get; set; }
    public string SourceKey { get; set; } = "";
    public string SourceData { get; set; } = "";
    public DateTimeOffset TimeAdded { get; set; } = DateTimeOffset.MaxValue;
    public DateTimeOffset TimeActive { get; set; } = DateTimeOffset.MaxValue;
    public DateTimeOffset TimeInactive { get; set; } = DateTimeOffset.MinValue;
    public DateTimeOffset TimeToDelete { get; set; } = DateTimeOffset.MaxValue;
}
