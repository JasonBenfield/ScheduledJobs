namespace XTI_JobsDB.EF;

public sealed class TriggeredJobEntity
{
    public int ID { get; set; }
    public int JobDefinitionID { get; set; }
    public int EventNotificationID { get; set; }
    public DateTimeOffset TimeInactive { get; set; } = DateTimeOffset.MaxValue;
}
