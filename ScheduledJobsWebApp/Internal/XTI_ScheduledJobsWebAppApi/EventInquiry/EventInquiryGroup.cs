namespace XTI_ScheduledJobsWebAppApi.EventInquiry;

public sealed class EventInquiryGroup : AppApiGroupWrapper
{
    public EventInquiryGroup(AppApiGroup source, IServiceProvider sp)
        : base(source)
    {
        Notifications = source.AddAction
        (
            nameof(Notifications), () => sp.GetRequiredService<NotificationsView>()
        );
        GetRecentNotifications = source.AddAction
        (
            nameof(GetRecentNotifications), () => sp.GetRequiredService<GetRecentNotificationsAction>()
        );
        NotificationDetail = source.AddAction
        (
            nameof(NotificationDetail), () => sp.GetRequiredService<NotificationDetailView>()
        );
        GetNotificationDetail = source.AddAction
        (
            nameof(GetNotificationDetail), () => sp.GetRequiredService<GetNotificationDetailAction>()
        );
    }

    public AppApiAction<EmptyRequest, WebViewResult> Notifications { get; }
    public AppApiAction<EmptyRequest, EventSummaryModel[]> GetRecentNotifications { get; }
    public AppApiAction<GetNotificationDetailRequest, WebViewResult> NotificationDetail { get; }
    public AppApiAction<GetNotificationDetailRequest, EventNotificationDetailModel> GetNotificationDetail { get; }
}