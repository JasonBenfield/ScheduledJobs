namespace XTI_Jobs.Abstractions;

public sealed class GetRecentEventNotificationsByEventDefinitionRequest
{
    public GetRecentEventNotificationsByEventDefinitionRequest()
        : this(0, "")
    {
    }

    public GetRecentEventNotificationsByEventDefinitionRequest(int eventDefinitionID, string sourceKey)
    {
        EventDefinitionID = eventDefinitionID;
        SourceKey = sourceKey;
    }

    public int EventDefinitionID { get; set; }
    public string SourceKey { get; set; }
}
