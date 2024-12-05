namespace XTI_ScheduledJobsWebAppApi.EventInquiry;

public sealed class EventInquiryGroup : AppApiGroupWrapper
{
    public EventInquiryGroup(AppApiGroup source, IServiceProvider sp)
        : base(source)
    {
        Notifications = source.AddAction<EmptyRequest, WebViewResult>()
            .Named(nameof(Notifications))
            .WithExecution<NotificationsView>()
            .Build();
        GetRecentNotifications = source.AddAction<EmptyRequest, EventSummaryModel[]>()
            .Named(nameof(GetRecentNotifications))
            .WithExecution<GetRecentNotificationsAction>()
            .Build();
        NotificationDetail = source.AddAction<GetNotificationDetailRequest, WebViewResult>()
            .Named(nameof(NotificationDetail))
            .WithExecution<NotificationDetailView>()
            .Build();
        GetNotificationDetail = source.AddAction<GetNotificationDetailRequest, EventNotificationDetailModel>()
            .Named(nameof(GetNotificationDetail))
            .WithExecution<GetNotificationDetailAction>()
            .Build();
    }

    public AppApiAction<EmptyRequest, WebViewResult> Notifications { get; }
    public AppApiAction<EmptyRequest, EventSummaryModel[]> GetRecentNotifications { get; }
    public AppApiAction<GetNotificationDetailRequest, WebViewResult> NotificationDetail { get; }
    public AppApiAction<GetNotificationDetailRequest, EventNotificationDetailModel> GetNotificationDetail { get; }
}