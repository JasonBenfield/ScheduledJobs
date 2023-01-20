namespace XTI_Jobs;

public interface ITransformedEventData
{
    Task<string> TransformEventData(string sourceKey, string sourceData);
}
