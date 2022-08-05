using XTI_ScheduledJobsWebAppApi.JobDefinitions;

namespace XTI_ScheduledJobsWebAppApi;

partial class ScheduledJobsAppApi
{
    private JobDefinitionsGroup? _JobDefinitions;

    public JobDefinitionsGroup JobDefinitions { get => _JobDefinitions ?? throw new ArgumentNullException(nameof(_JobDefinitions)); }

    partial void createJobDefinitionsGroup(IServiceProvider sp)
    {
        _JobDefinitions = new JobDefinitionsGroup
        (
            source.AddGroup(nameof(JobDefinitions)),
            sp
        );
    }
}