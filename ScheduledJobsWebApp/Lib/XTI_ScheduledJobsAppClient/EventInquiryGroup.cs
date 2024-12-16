// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class EventInquiryGroup : AppClientGroup
{
    public EventInquiryGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl, AppClientOptions options) : base(httpClientFactory, xtiTokenAccessor, clientUrl, options, "EventInquiry")
    {
        Actions = new EventInquiryGroupActions(GetNotificationDetail: CreatePostAction<GetNotificationDetailRequest, EventNotificationDetailModel>("GetNotificationDetail"), GetRecentNotifications: CreatePostAction<EmptyRequest, EventSummaryModel[]>("GetRecentNotifications"), NotificationDetail: CreateGetAction<GetNotificationDetailRequest>("NotificationDetail"), Notifications: CreateGetAction<EmptyRequest>("Notifications"));
    }

    public EventInquiryGroupActions Actions { get; }

    public Task<EventNotificationDetailModel> GetNotificationDetail(GetNotificationDetailRequest model, CancellationToken ct = default) => Actions.GetNotificationDetail.Post("", model, ct);
    public Task<EventSummaryModel[]> GetRecentNotifications(CancellationToken ct = default) => Actions.GetRecentNotifications.Post("", new EmptyRequest(), ct);
    public sealed record EventInquiryGroupActions(AppClientPostAction<GetNotificationDetailRequest, EventNotificationDetailModel> GetNotificationDetail, AppClientPostAction<EmptyRequest, EventSummaryModel[]> GetRecentNotifications, AppClientGetAction<GetNotificationDetailRequest> NotificationDetail, AppClientGetAction<EmptyRequest> Notifications);
}