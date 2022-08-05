using XTI_DB;

namespace XTI_JobsDB.EF;

public sealed class JobDbName : XtiDbName
{
    public JobDbName(string environmentName) : base(environmentName, "Jobs")
    {
    }
}