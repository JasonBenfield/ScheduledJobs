using XTI_App.Abstractions;

namespace ScheduledJobsSetupApp;

internal sealed class ScheduledJobsAppSetup : IAppSetup
{
    public Task Run(AppVersionKey versionKey)
    {
        return Task.CompletedTask;
    }
}
