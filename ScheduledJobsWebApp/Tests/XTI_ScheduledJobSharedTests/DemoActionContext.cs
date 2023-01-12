using System;

namespace XTI_ScheduledJobSharedTests;

public sealed class DemoActionContext<T> where T : IJobAction
{
    public int NumberOfTimesExecuted { get; set; }
    public int TargetID { get; set; }
    public string Output { get; set; } = "";
    public string[] Messages { get; set; } = new string[0];
    public TimeSpan Delay { get; set; } = TimeSpan.Zero;
}
