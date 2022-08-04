// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class EventInquiryGroup : AppClientGroup
{
    public EventInquiryGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl, AppClientOptions options) : base(httpClientFactory, xtiTokenAccessor, clientUrl, options, "EventInquiry")
    {
        Actions = new EventInquiryGroupActions(Notifications: CreateGetAction<EmptyRequest>("Notifications"), GetRecentNotifications: CreatePostAction<EmptyRequest, EventSummaryModel[]>("GetRecentNotifications"), NotificationDetail: CreateGetAction<GetNotificationDetailRequest>("NotificationDetail"), GetNotificationDetail: CreatePostAction<GetNotificationDetailRequest, EventNotificationDetailModel>("GetNotificationDetail"));
    }

    public EventInquiryGroupActions Actions { get; }

    public Task<EventSummaryModel[]> GetRecentNotifications() => Actions.GetRecentNotifications.Post("", new EmptyRequest());
    public Task<EventNotificationDetailModel> GetNotificationDetail(GetNotificationDetailRequest model) => Actions.GetNotificationDetail.Post("", model);
    public sealed record EventInquiryGroupActions(AppClientGetAction<EmptyRequest> Notifications, AppClientPostAction<EmptyRequest, EventSummaryModel[]> GetRecentNotifications, AppClientGetAction<GetNotificationDetailRequest> NotificationDetail, AppClientPostAction<GetNotificationDetailRequest, EventNotificationDetailModel> GetNotificationDetail);
}