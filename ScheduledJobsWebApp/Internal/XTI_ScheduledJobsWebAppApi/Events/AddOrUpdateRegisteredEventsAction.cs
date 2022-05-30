namespace XTI_ScheduledJobsWebAppApi.Events;

public sealed class AddOrUpdateRegisteredEventsAction : AppAction<RegisteredEvent[], EmptyActionResult>
{
    private readonly IJobDb db;

    public AddOrUpdateRegisteredEventsAction(IJobDb db)
    {
        this.db = db;
    }

    public async Task<EmptyActionResult> Execute(RegisteredEvent[] registeredEvents)
    {
        await db.AddOrUpdateRegisteredEvents(registeredEvents);
        return new EmptyActionResult();
    }
}