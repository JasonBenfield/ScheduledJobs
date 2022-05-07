namespace XTI_Jobs;

public sealed class EventRegistration
{
    private readonly IStoredEvents storedEvents;
    private readonly List<EventBuilder> events = new();

    public EventRegistration(IStoredEvents storedEvents)
    {
        this.storedEvents = storedEvents;
    }

    public EventRegistration AddEvent(EventKey eventKey) =>
        AddEvent(eventKey, _ => { });

    public EventRegistration AddEvent(EventKey eventKey, Action<EventBuilder> config)
    {
        var builder = new EventBuilder(eventKey);
        events.Add(builder);
        config(builder);
        return this;
    }

    public Task Register()
    {
        var registeredEvents = events.Select(evt => evt.Build()).ToArray();
        return storedEvents.AddOrUpdateRegisteredEvents(registeredEvents);
    }
}
