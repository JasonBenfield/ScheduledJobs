using XTI_App.Abstractions;
using XTI_DB;
using XTI_Hub.Abstractions;
using XTI_HubAppClient;
using XTI_JobsDB.EF;
using XTI_ScheduledJobsWebAppApi;

namespace ScheduledJobsSetupApp;

internal sealed class ScheduledJobsAppSetup : IAppSetup
{
    private readonly DbAdmin<JobDbContext> dbAdmin;
    private readonly HubAppClient hubClient;

    public ScheduledJobsAppSetup(DbAdmin<JobDbContext> dbAdmin, HubAppClient hubClient)
    {
        this.dbAdmin = dbAdmin;
        this.hubClient = hubClient;
    }

    public async Task Run(AppVersionKey versionKey)
    {
        await dbAdmin.Update();
        var systemUserName = new SystemUserName(ScheduledJobsInfo.AppKey, Environment.MachineName);
        await hubClient.Install.SetUserAccess
        (
            new SetUserAccessRequest
            (
                systemUserName.UserName,
                new SetUserAccessRoleRequest
                (
                    AppKey.WebApp(hubClient.AppName),
                    new AppRoleName(hubClient.RoleNames.ViewLog)
                )
            )
        );
    }
}
