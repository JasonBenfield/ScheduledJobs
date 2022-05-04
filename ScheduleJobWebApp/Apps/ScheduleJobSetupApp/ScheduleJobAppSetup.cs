using XTI_App.Abstractions;

namespace ScheduleJobSetupApp;

internal sealed class ScheduleJobAppSetup : IAppSetup
{
    public Task Run(AppVersionKey versionKey)
    {
        return Task.CompletedTask;
    }
}
