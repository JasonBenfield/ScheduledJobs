namespace XTI_Jobs;

public sealed class EventRegistrationBuilder1
{
    private readonly EventRegistrationBuilder builder;
    private readonly EventKey eventKey;
    private TimeSpan deleteAfter = TimeSpan.FromDays(365);
    private TimeSpan activeFor = TimeSpan.MaxValue;

    internal EventRegistrationBuilder1(EventRegistrationBuilder builder, EventKey eventKey)
    {
        this.builder = builder;
        this.eventKey = eventKey;
    }

    private DateTimeOffset TimeToStartNotification { get; set; } = DateTimeOffset.MinValue;
    internal bool CompareSourceKeyAndDataForDuplication { get; set; } = true;
    internal DuplicateHandling DuplicateHandling { get; set; } = DuplicateHandling.Values.Ignore;

    public EventDuplicationBuilder IgnoreDuplicates() => Duplicates(DuplicateHandling.Values.Ignore);

    public EventDuplicationBuilder KeepNewest() => Duplicates(DuplicateHandling.Values.KeepNewest);

    public EventDuplicationBuilder KeepOldest() => Duplicates(DuplicateHandling.Values.KeepOldest);

    public EventRegistrationBuilder1 KeepAll()
    {
        Duplicates(DuplicateHandling.Values.KeepAll);
        return this;
    }

    private EventDuplicationBuilder Duplicates(DuplicateHandling duplicateHandling)
    {
        DuplicateHandling = duplicateHandling;
        return new EventDuplicationBuilder(this);
    }

    public EventRegistrationBuilder1 StartNotifying(DateTimeOffset timeToStartNotifications)
    {
        TimeToStartNotification = timeToStartNotifications;
        return this;
    }

    public EventRegistrationBuilder1 ActiveFor(TimeSpan activeFor)
    {
        this.activeFor = activeFor;
        return this;
    }

    public EventRegistrationBuilder1 DeleteAfter(TimeSpan deleteAfter)
    {
        this.deleteAfter = deleteAfter;
        return this;
    }

    public EventRegistrationBuilder1 AddEvent(EventKey eventKey) => builder.AddEvent(eventKey);

    public EventRegistration Build() => builder.Build();

    internal RegisteredEvent BuildEvent() => 
        new RegisteredEvent
        (
            eventKey, 
            CompareSourceKeyAndDataForDuplication, 
            DuplicateHandling, 
            TimeToStartNotification,
            activeFor,
            deleteAfter
        );
}