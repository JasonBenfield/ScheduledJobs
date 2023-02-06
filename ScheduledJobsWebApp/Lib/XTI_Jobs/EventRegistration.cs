namespace XTI_Jobs;

public sealed class EventRegistration
{
    private readonly IJobDb db;
    private readonly EventRegistrationBuilder1[] events;

    public EventRegistration(IJobDb db, EventRegistrationBuilder1[] events)
    {
        this.db = db;
        this.events = events;
    }

    public Task Register()
    {
        var registeredEvents = events.Select(evt => evt.BuildEvent()).ToArray();
        return db.AddOrUpdateRegisteredEvents(registeredEvents);
    }
}

public sealed class EventRegistrationBuilder
{
    private readonly IJobDb db;
    private readonly List<EventRegistrationBuilder1> events = new();

    public EventRegistrationBuilder(IJobDb db)
    {
        this.db = db;
    }

    public EventRegistrationBuilder1 AddEvent(EventKey eventKey)
    {
        var evt = new EventRegistrationBuilder1(this, eventKey);
        events.Add(evt);
        return evt;
    }

    internal EventRegistration Build() => new EventRegistration(db, events.ToArray());
}
