// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class EventInquiryActions
{
    internal EventInquiryActions(AppClientUrl appClientUrl)
    {
        Notifications = new AppClientGetAction<EmptyRequest>(appClientUrl, "Notifications");
        NotificationDetail = new AppClientGetAction<GetNotificationDetailRequest>(appClientUrl, "NotificationDetail");
    }

    public AppClientGetAction<EmptyRequest> Notifications { get; }

    public AppClientGetAction<GetNotificationDetailRequest> NotificationDetail { get; }
}