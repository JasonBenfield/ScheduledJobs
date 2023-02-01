namespace XTI_Jobs;

public sealed class EventRegistration
{
    private readonly IJobDb db;
    private readonly List<RegisteredEventBuilder> events = new();

    public EventRegistration(IJobDb db)
    {
        this.db = db;
    }

    public EventRegistration AddEvent(EventKey eventKey) =>
        AddEvent(eventKey, _ => { });

    public EventRegistration AddEvent(EventKey eventKey, Action<RegisteredEventBuilder> config)
    {
        var builder = new RegisteredEventBuilder(eventKey);
        events.Add(builder);
        config(builder);
        return this;
    }

    public Task Register()
    {
        var registeredEvents = events.Select(evt => evt.Build()).ToArray();
        return db.AddOrUpdateRegisteredEvents(registeredEvents);
    }
}
