namespace XTI_Jobs;

public sealed class EventDuplicationBuilder
{
    private readonly RegisteredEventBuilder eventBuilder;

    internal EventDuplicationBuilder(RegisteredEventBuilder eventBuilder)
    {
        this.eventBuilder = eventBuilder;
    }

    public RegisteredEventBuilder WhenSourceKeysOnlyAreEqual()
    {
        eventBuilder.CompareSourceKeyAndDataForDuplication = false;
        return eventBuilder;
    }

    public RegisteredEventBuilder WhenSourceKeysAndDataAreEqual()
    {
        eventBuilder.CompareSourceKeyAndDataForDuplication = true;
        return eventBuilder;
    }
}
