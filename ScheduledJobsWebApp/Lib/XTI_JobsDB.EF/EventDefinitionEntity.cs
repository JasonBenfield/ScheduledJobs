namespace XTI_JobsDB.Entities;

public sealed class EventDefinitionEntity
{
    public int ID { get; set; }
    public string EventKey { get; set; } = "";
    public string DisplayText { get; set; } = "";
    public bool CompareSourceKeyAndDataForDuplication { get; set; }
    public int DuplicateHandling { get; set; }
    public DateTimeOffset TimeToStartNotifications { get; set; } = DateTimeOffset.MinValue;
    public TimeSpan ActiveFor { get; set; } = TimeSpan.Zero;
    public TimeSpan DeleteAfter { get; set; } = TimeSpan.Zero;
}
