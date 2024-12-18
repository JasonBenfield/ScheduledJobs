using XTI_ScheduledJobsWebAppApiActions.JobDefinitions;

// Generated Code
#nullable enable
namespace XTI_ScheduledJobsWebAppApi.JobDefinitions;
public sealed partial class JobDefinitionsGroupBuilder
{
    private readonly AppApiGroup source;
    internal JobDefinitionsGroupBuilder(AppApiGroup source)
    {
        this.source = source;
        GetJobDefinitions = source.AddAction<EmptyRequest, JobDefinitionModel[]>("GetJobDefinitions").WithExecution<GetJobDefinitionsAction>();
        GetRecentTriggeredJobs = source.AddAction<GetRecentTriggeredJobsByDefinitionRequest, JobSummaryModel[]>("GetRecentTriggeredJobs").WithExecution<GetRecentTriggeredJobsAction>();
        Index = source.AddAction<EmptyRequest, WebViewResult>("Index").WithExecution<IndexPage>();
        Configure();
    }

    partial void Configure();
    public AppApiActionBuilder<EmptyRequest, JobDefinitionModel[]> GetJobDefinitions { get; }
    public AppApiActionBuilder<GetRecentTriggeredJobsByDefinitionRequest, JobSummaryModel[]> GetRecentTriggeredJobs { get; }
    public AppApiActionBuilder<EmptyRequest, WebViewResult> Index { get; }

    public JobDefinitionsGroup Build() => new JobDefinitionsGroup(source, this);
}