namespace XTI_Jobs;

public sealed class EventDuplicationBuilder
{
    private readonly EventRegistrationBuilder1 eventBuilder;

    internal EventDuplicationBuilder(EventRegistrationBuilder1 eventBuilder)
    {
        this.eventBuilder = eventBuilder;
    }

    public EventRegistrationBuilder1 WhenSourceKeysOnlyAreEqual()
    {
        eventBuilder.CompareSourceKeyAndDataForDuplication = false;
        return eventBuilder;
    }

    public EventRegistrationBuilder1 WhenSourceKeysAndDataAreEqual()
    {
        eventBuilder.CompareSourceKeyAndDataForDuplication = true;
        return eventBuilder;
    }
}
