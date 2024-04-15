namespace XTI_Jobs.Abstractions;

public sealed class TriggeredJobsRequest
{
    public TriggeredJobsRequest()
        : this(0)
    {
    }

    public TriggeredJobsRequest(int eventNotificationID)
    {
        EventNotificationID = eventNotificationID;
    }

    public int EventNotificationID { get; set; }
}
