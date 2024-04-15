namespace XTI_Jobs.Abstractions;

public sealed class GetRecentTriggeredJobsByDefinitionRequest
{
    public GetRecentTriggeredJobsByDefinitionRequest()
        : this(0)
    {
    }

    public GetRecentTriggeredJobsByDefinitionRequest(int jobDefinitionID)
    {
        JobDefinitionID = jobDefinitionID;
    }

    public int JobDefinitionID { get; set; }
}
