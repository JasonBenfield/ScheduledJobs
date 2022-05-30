using XTI_ScheduledJobsWebAppApi.Jobs;

namespace XTI_ScheduledJobsWebAppApi;

partial class ScheduledJobsAppApi
{
    private JobsGroup? _Jobs;

    public JobsGroup Jobs { get => _Jobs ?? throw new ArgumentNullException(nameof(_Jobs)); }

    partial void createJobsGroup(IServiceProvider sp)
    {
        _Jobs = new JobsGroup
        (
            source.AddGroup(nameof(Jobs)),
            sp
        );
    }
}