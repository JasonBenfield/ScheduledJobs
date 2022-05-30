using System;

namespace XTI_ScheduledJobSharedTests;

public sealed class DemoItemActionException : Exception
{
    public DemoItemActionException(string message) : base(message)
    {
    }
}
