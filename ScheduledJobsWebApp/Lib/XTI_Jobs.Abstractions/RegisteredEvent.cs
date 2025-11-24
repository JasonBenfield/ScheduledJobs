namespace XTI_Jobs.Abstractions;

public sealed record RegisteredEvent
(
    EventKey EventKey,
    bool CompareSourceKeyAndDataForDuplication,
    DuplicateHandling DuplicateHandling,
    DateTimeOffset TimeToStartNotifications,
    TimeSpan ActiveFor,
    TimeSpan DeleteAfter
)
{
    public RegisteredEvent()
        : this
        (
             EventKey: new(),
             CompareSourceKeyAndDataForDuplication: false,
             DuplicateHandling: DuplicateHandling.Values.Ignore,
             TimeToStartNotifications: DateTimeOffset.MaxValue,
             ActiveFor: new TimeSpan(),
             DeleteAfter: new TimeSpan()
        )
    {
    }
}
