namespace XTI_ScheduledJobsWebAppApi.Events;

internal sealed class AddNotificationsAction : AppAction<AddNotificationsRequest, EventNotificationModel[]>
{
    private readonly IJobDb db;

    public AddNotificationsAction(IJobDb db)
    {
        this.db = db;
    }

    public Task<EventNotificationModel[]> Execute(AddNotificationsRequest addRequest, CancellationToken ct) =>
        db.AddEventNotifications(new EventKey(addRequest.EventKey), addRequest.Sources);
}
