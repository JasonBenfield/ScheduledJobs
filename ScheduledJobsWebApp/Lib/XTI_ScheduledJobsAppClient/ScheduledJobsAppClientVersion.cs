// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class ScheduledJobsAppClientVersion
{
    public static ScheduledJobsAppClientVersion Version(string value) => new ScheduledJobsAppClientVersion(value);
    public ScheduledJobsAppClientVersion(IHostEnvironment hostEnv) : this(getValue(hostEnv))
    {
    }

    private static string getValue(IHostEnvironment hostEnv)
    {
        string value;
        if (hostEnv.IsProduction())
        {
            value = "V26";
        }
        else
        {
            value = "Current";
        }

        return value;
    }

    private ScheduledJobsAppClientVersion(string value)
    {
        Value = value;
    }

    public string Value { get; }
}