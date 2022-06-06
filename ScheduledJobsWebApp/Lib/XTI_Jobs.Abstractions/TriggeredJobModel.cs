namespace XTI_Jobs.Abstractions;

public sealed record TriggeredJobModel(int ID, JobDefinitionModel JobDefinition, int EventNotificationID)
{
    public TriggeredJobModel()
        : this(0, new JobDefinitionModel(), 0)
    {
    }
}