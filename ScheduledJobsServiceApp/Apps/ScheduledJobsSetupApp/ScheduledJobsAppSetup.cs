using XTI_App.Abstractions;
using XTI_Hub.Abstractions;
using XTI_HubAppClient;
using XTI_ScheduledJobsServiceAppApi;

namespace ScheduledJobsSetupApp;

internal sealed class ScheduledJobsAppSetup : IAppSetup
{
    private readonly HubAppClient hubClient;

    public ScheduledJobsAppSetup(HubAppClient hubClient)
    {
        this.hubClient = hubClient;
    }

    public async Task Run(AppVersionKey versionKey)
    {
        var systemUserName = new SystemUserName(ScheduledJobsInfo.AppKey, Environment.MachineName);
        await hubClient.Install.SetUserAccess
        (
            new SetUserAccessRequest
            (
                systemUserName.UserName,
                new SetUserAccessRoleRequest(AppKey.WebApp("ScheduledJobs"), AppRoleName.Admin)
            )
        );
    }
}
