namespace XTI_ScheduledJobsWebAppApi.EventInquiry;

public sealed class EventInquiryGroup : AppApiGroupWrapper
{
    public EventInquiryGroup(AppApiGroup source, IServiceProvider sp)
        : base(source)
    {
        var actions = new AppApiActionFactory(source);
        Notifications = source.AddAction
        (
            actions.Action(nameof(Notifications), () => sp.GetRequiredService<NotificationsView>())
        );
        GetRecentNotifications = source.AddAction
        (
            actions.Action(nameof(GetRecentNotifications), () => sp.GetRequiredService<GetRecentNotificationsAction>())
        );
        NotificationDetail = source.AddAction
        (
            actions.Action(nameof(NotificationDetail), () => sp.GetRequiredService<NotificationDetailView>())
        );
        GetNotificationDetail = source.AddAction
        (
            actions.Action(nameof(GetNotificationDetail), () => sp.GetRequiredService<GetNotificationDetailAction>())
        );
    }

    public AppApiAction<EmptyRequest, WebViewResult> Notifications { get; }
    public AppApiAction<EmptyRequest, EventSummaryModel[]> GetRecentNotifications { get; }
    public AppApiAction<GetNotificationDetailRequest, WebViewResult> NotificationDetail { get; }
    public AppApiAction<GetNotificationDetailRequest, EventNotificationDetailModel> GetNotificationDetail { get; }
}