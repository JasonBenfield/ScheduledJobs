namespace XTI_ScheduledJobSharedTests;

public static class DemoEventKeys
{
    public static readonly EventKey SomethingHappened = new(nameof(SomethingHappened));
    public static readonly EventKey SomethingElseHappened = new(nameof(SomethingElseHappened));
}
