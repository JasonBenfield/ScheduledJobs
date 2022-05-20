namespace XTI_ScheduledJobTests;

internal sealed class DoSomethingData
{
    public int SourceID { get; set; }
    public int TargetID { get; set; }
    public string Action01 { get; set; } = "";
    public string Action02 { get; set; } = "";
    public string Output { get; set; } = "";
    public DoSomethingItemData[] Items { get; set; } = new DoSomethingItemData[0];
}
