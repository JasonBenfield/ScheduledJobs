using XTI_App.Abstractions;
using XTI_DB;
using XTI_JobsDB.EF;

namespace ScheduledJobsSetupApp;

internal sealed class ScheduledJobsAppSetup : IAppSetup
{
    private readonly DbAdmin<JobDbContext> dbAdmin;

    public ScheduledJobsAppSetup(DbAdmin<JobDbContext> dbAdmin)
    {
        this.dbAdmin = dbAdmin;
    }

    public Task Run(AppVersionKey versionKey)
    {
        return dbAdmin.Update();
    }
}
