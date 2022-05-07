using XTI_DB;

namespace XTI_JobsDB.EF;

public sealed class JobConnectionString : XtiConnectionString
{
    public JobConnectionString(DbOptions options, string envName)
        : base(options, new JobDbName(envName))
    {
    }
}