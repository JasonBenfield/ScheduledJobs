namespace XTI_Jobs;

public sealed class UnchangedEventData : ITransformedEventData
{
    public Task<string> TransformEventData(string sourceKey, string sourceData) =>
        Task.FromResult(sourceData);
}
