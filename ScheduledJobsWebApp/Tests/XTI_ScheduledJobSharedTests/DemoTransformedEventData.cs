using System.Text.Json;

namespace XTI_ScheduledJobSharedTests;

public sealed class DemoTransformedEventData : ITransformedEventData
{
    private bool errorDuringTransform;

    public void FailTransformSourceData()
    {
        errorDuringTransform = true;
    }

    public void AllowTransformSourceData()
    {
        errorDuringTransform = false;
    }

    public Task<string> TransformEventData(string sourceKey, string sourceData)
    {
        if (errorDuringTransform)
        {
            throw new Exception();
        }
        var somethingHappenedData = JsonSerializer.Deserialize<SomethingHappenedData>(sourceData) ?? new SomethingHappenedData();
        var doSomethingData = new DoSomethingData
        {
            SourceID = somethingHappenedData.ID,
            TargetID = somethingHappenedData.ID * 10,
            Items = somethingHappenedData.Items.Select(item => new DoSomethingItemData { ItemID = item }).ToArray()
        };
        var serializedTargetData = JsonSerializer.Serialize(doSomethingData);
        return Task.FromResult(serializedTargetData);
    }
}
