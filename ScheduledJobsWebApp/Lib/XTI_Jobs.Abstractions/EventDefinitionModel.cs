namespace XTI_Jobs.Abstractions;

public sealed record EventDefinitionModel(int ID, EventKey EventKey)
{
    public EventDefinitionModel()
        : this(0, new EventKey(""))
    {
    }
}