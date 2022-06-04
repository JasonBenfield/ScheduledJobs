using XTI_ScheduledJobsServiceAppApi.Jobs;

namespace XTI_ScheduledJobsServiceAppApi;

partial class ScheduledJobsAppApi
{
    private JobsGroup? jobs;

    public JobsGroup Jobs { get => jobs ?? throw new ArgumentNullException(nameof(jobs)); }

    partial void createJobsGroup(IServiceProvider sp)
    {
        jobs = new JobsGroup
        (
            source.AddGroup(nameof(Jobs)),
            sp
        );
    }
}