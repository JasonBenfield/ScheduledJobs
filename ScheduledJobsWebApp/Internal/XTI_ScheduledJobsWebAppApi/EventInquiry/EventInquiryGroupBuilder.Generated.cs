using XTI_ScheduledJobsWebAppApiActions.EventInquiry;

// Generated Code
#nullable enable
namespace XTI_ScheduledJobsWebAppApi.EventInquiry;
public sealed partial class EventInquiryGroupBuilder
{
    private readonly AppApiGroup source;
    internal EventInquiryGroupBuilder(AppApiGroup source)
    {
        this.source = source;
        GetNotificationDetail = source.AddAction<GetNotificationDetailRequest, EventNotificationDetailModel>("GetNotificationDetail").WithExecution<GetNotificationDetailAction>();
        GetRecentNotifications = source.AddAction<EmptyRequest, EventSummaryModel[]>("GetRecentNotifications").WithExecution<GetRecentNotificationsAction>();
        NotificationDetail = source.AddAction<GetNotificationDetailRequest, WebViewResult>("NotificationDetail").WithExecution<NotificationDetailPage>();
        Notifications = source.AddAction<EmptyRequest, WebViewResult>("Notifications").WithExecution<NotificationsPage>();
        Configure();
    }

    partial void Configure();
    public AppApiActionBuilder<GetNotificationDetailRequest, EventNotificationDetailModel> GetNotificationDetail { get; }
    public AppApiActionBuilder<EmptyRequest, EventSummaryModel[]> GetRecentNotifications { get; }
    public AppApiActionBuilder<GetNotificationDetailRequest, WebViewResult> NotificationDetail { get; }
    public AppApiActionBuilder<EmptyRequest, WebViewResult> Notifications { get; }

    public EventInquiryGroup Build() => new EventInquiryGroup(source, this);
}