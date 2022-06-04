using XTI_Core;

namespace XTI_ScheduledJobsWebAppApi.Events;

internal sealed class AddNotificationsAction : AppAction<AddNotificationsRequest, EventNotificationModel[]>
{
    private readonly IJobDb db;

    public AddNotificationsAction(IJobDb db)
    {
        this.db = db;
    }

    public Task<EventNotificationModel[]> Execute(AddNotificationsRequest model, CancellationToken ct) =>
        db.AddEventNotifications(model.EventKey, model.Sources);
}
