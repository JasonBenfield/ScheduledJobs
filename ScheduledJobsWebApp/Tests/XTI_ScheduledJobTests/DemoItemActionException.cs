using System;

namespace XTI_ScheduledJobTests;

public sealed class DemoItemActionException : Exception
{
    public DemoItemActionException(string message) : base(message)
    {
    }
}
