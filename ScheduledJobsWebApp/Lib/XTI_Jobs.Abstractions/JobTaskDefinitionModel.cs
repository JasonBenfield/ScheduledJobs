namespace XTI_Jobs.Abstractions;

public sealed record JobTaskDefinitionModel(int ID, JobTaskKey TaskKey)
{
    public JobTaskDefinitionModel()
        : this(0, new JobTaskKey())
    {
    }
}