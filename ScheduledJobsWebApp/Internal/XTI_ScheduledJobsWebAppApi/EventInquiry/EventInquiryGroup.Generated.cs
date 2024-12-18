using XTI_ScheduledJobsWebAppApiActions.EventInquiry;

// Generated Code
#nullable enable
namespace XTI_ScheduledJobsWebAppApi.EventInquiry;
public sealed partial class EventInquiryGroup : AppApiGroupWrapper
{
    internal EventInquiryGroup(AppApiGroup source, EventInquiryGroupBuilder builder) : base(source)
    {
        GetNotificationDetail = builder.GetNotificationDetail.Build();
        GetRecentNotifications = builder.GetRecentNotifications.Build();
        NotificationDetail = builder.NotificationDetail.Build();
        Notifications = builder.Notifications.Build();
        Configure();
    }

    partial void Configure();
    public AppApiAction<GetNotificationDetailRequest, EventNotificationDetailModel> GetNotificationDetail { get; }
    public AppApiAction<EmptyRequest, EventSummaryModel[]> GetRecentNotifications { get; }
    public AppApiAction<GetNotificationDetailRequest, WebViewResult> NotificationDetail { get; }
    public AppApiAction<EmptyRequest, WebViewResult> Notifications { get; }
}