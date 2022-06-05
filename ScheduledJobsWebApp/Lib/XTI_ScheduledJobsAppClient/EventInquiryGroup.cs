// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class EventInquiryGroup : AppClientGroup
{
    public EventInquiryGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl) : base(httpClientFactory, xtiTokenAccessor, clientUrl, "EventInquiry")
    {
        Actions = new EventInquiryActions(clientUrl);
    }

    public EventInquiryActions Actions { get; }

    public Task<EventSummaryModel[]> GetRecentNotifications() => Post<EventSummaryModel[], EmptyRequest>("GetRecentNotifications", "", new EmptyRequest());
    public Task<EventNotificationDetailModel> GetNotificationDetail(GetNotificationDetailRequest model) => Post<EventNotificationDetailModel, GetNotificationDetailRequest>("GetNotificationDetail", "", model);
}