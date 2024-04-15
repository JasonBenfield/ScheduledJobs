namespace XTI_Jobs.Abstractions;

public sealed class GetNotificationDetailRequest
{
    public GetNotificationDetailRequest()
        : this(0)
    {
    }

    public GetNotificationDetailRequest(int notificationID)
    {
        NotificationID = notificationID;
    }

    public int NotificationID { get; set; }
}
