namespace XTI_Jobs.Abstractions;

public sealed record JobDefinitionModel(int ID, JobKey JobKey)
{
    public JobDefinitionModel()
        : this(0, new JobKey())
    {
    }
}