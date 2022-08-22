namespace XTI_ScheduledJobSharedTests;

public sealed record DoSomethingItemData
{
    public int ItemID { get; set; }
    public string Value { get; set; } = "";
    public string AnotherValue { get; set; } = "";
}
