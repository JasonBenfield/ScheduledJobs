using System.Collections.Generic;

namespace XTI_ScheduledJobTests;

internal sealed class DemoActionContext<T> where T : IJobAction
{
    public int NumberOfTimesExecuted { get; set; }
    public int TargetID { get; set; }
    public string Output { get; set; } = "";
}
