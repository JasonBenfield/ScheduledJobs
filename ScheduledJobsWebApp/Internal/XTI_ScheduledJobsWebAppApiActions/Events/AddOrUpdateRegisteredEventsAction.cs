namespace XTI_ScheduledJobsWebAppApiActions.Events;

public sealed class AddOrUpdateRegisteredEventsAction : AppAction<RegisteredEvent[], EmptyActionResult>
{
    private readonly IJobDb db;

    public AddOrUpdateRegisteredEventsAction(IJobDb db)
    {
        this.db = db;
    }

    public async Task<EmptyActionResult> Execute(RegisteredEvent[] registeredEvents, CancellationToken ct)
    {
        await db.AddOrUpdateRegisteredEvents(registeredEvents);
        return new EmptyActionResult();
    }
}