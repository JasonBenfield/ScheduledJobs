namespace XTI_Jobs.Abstractions;

public sealed record TriggeredJobDetailModel
(
    TriggeredJobModel Job,
    EventNotificationModel TriggeredBy,
    TriggeredJobTaskModel[] Tasks,
    SourceLogEntryModel[] SourceLogEntries
)
{
    public TriggeredJobDetailModel()
        : this(new(), new(), [], [])
    {
    }
}
