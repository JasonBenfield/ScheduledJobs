namespace XTI_Jobs.Abstractions;

public sealed class AddNotificationsRequest
{
    public AddNotificationsRequest()
        : this(new EventKey(), new XtiEventSource[0])
    {
    }

    public AddNotificationsRequest(EventKey eventKey, params XtiEventSource[] sources)
    {
        EventKey = eventKey.DisplayText;
        Sources = sources;
    }

    public string EventKey { get; set; }
    public XtiEventSource[] Sources { get; set; }
}
