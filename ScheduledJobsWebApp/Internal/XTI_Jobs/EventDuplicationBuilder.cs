namespace XTI_Jobs;

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
