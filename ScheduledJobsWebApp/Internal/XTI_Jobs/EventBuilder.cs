namespace XTI_Jobs;

public sealed class EventBuilder
{
    private readonly EventKey eventKey;

    internal EventBuilder(EventKey eventKey)
    {
        this.eventKey = eventKey;
    }

    internal bool CompareSourceKeyAndDataForDuplication { get; set; } = true;
    internal DuplicateHandling DuplicateHandling { get; set; } = DuplicateHandling.Values.Ignore;

    public EventDuplicationBuilder Ignore() => Duplicates(DuplicateHandling.Values.Ignore);

    public EventDuplicationBuilder KeepNewest() => Duplicates(DuplicateHandling.Values.KeepNewest);

    public EventDuplicationBuilder KeepOldest() => Duplicates(DuplicateHandling.Values.KeepOldest);

    public EventDuplicationBuilder KeepAll() => Duplicates(DuplicateHandling.Values.KeepAll);

    private EventDuplicationBuilder Duplicates(DuplicateHandling duplicateHandling)
    {
        DuplicateHandling = duplicateHandling;
        return new EventDuplicationBuilder(this);
    }

    public RegisteredEvent Build() => new RegisteredEvent(eventKey, CompareSourceKeyAndDataForDuplication, DuplicateHandling);
}

public sealed class EventDuplicationBuilder
{
    private readonly EventBuilder eventBuilder;

    internal EventDuplicationBuilder(EventBuilder eventBuilder)
    {
        this.eventBuilder = eventBuilder;
    }

    public EventBuilder WhenSourceKeysOnlyAreEqual()
    {
        eventBuilder.CompareSourceKeyAndDataForDuplication = false;
        return eventBuilder;
    }

    public EventBuilder WhenSourceKeysAndDataAreEqual()
    {
        eventBuilder.CompareSourceKeyAndDataForDuplication = true;
        return eventBuilder;
    }
}
