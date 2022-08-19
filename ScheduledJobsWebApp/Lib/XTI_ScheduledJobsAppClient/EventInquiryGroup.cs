// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class EventInquiryGroup : AppClientGroup
{
    public EventInquiryGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl, AppClientOptions options) : base(httpClientFactory, xtiTokenAccessor, clientUrl, options, "EventInquiry")
    {
        Actions = new EventInquiryGroupActions(Notifications: CreateGetAction<EmptyRequest>("Notifications"), GetRecentNotifications: CreatePostAction<EmptyRequest, EventSummaryModel[]>("GetRecentNotifications"), NotificationDetail: CreateGetAction<GetNotificationDetailRequest>("NotificationDetail"), GetNotificationDetail: CreatePostAction<GetNotificationDetailRequest, EventNotificationDetailModel>("GetNotificationDetail"));
    }

    public EventInquiryGroupActions Actions { get; }

    public Task<EventSummaryModel[]> GetRecentNotifications(CancellationToken ct = default) => Actions.GetRecentNotifications.Post("", new EmptyRequest(), ct);
    public Task<EventNotificationDetailModel> GetNotificationDetail(GetNotificationDetailRequest model, CancellationToken ct = default) => Actions.GetNotificationDetail.Post("", model, ct);
    public sealed record EventInquiryGroupActions(AppClientGetAction<EmptyRequest> Notifications, AppClientPostAction<EmptyRequest, EventSummaryModel[]> GetRecentNotifications, AppClientGetAction<GetNotificationDetailRequest> NotificationDetail, AppClientPostAction<GetNotificationDetailRequest, EventNotificationDetailModel> GetNotificationDetail);
}