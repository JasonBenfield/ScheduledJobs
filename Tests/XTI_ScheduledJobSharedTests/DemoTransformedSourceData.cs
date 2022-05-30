using System.Text.Json;

namespace XTI_ScheduledJobSharedTests;

public sealed class DemoTransformedSourceData : ITransformedSourceData
{
    private readonly string sourceData;

    public DemoTransformedSourceData(string sourceData)
    {
        this.sourceData = sourceData;
    }

    public Task<string> Value()
    {
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
