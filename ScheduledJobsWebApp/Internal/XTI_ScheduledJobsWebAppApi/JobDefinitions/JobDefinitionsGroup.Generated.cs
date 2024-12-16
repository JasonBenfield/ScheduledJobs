using XTI_ScheduledJobsWebAppApiActions.JobDefinitions;

// Generated Code
#nullable enable
namespace XTI_ScheduledJobsWebAppApi.JobDefinitions;
public sealed partial class JobDefinitionsGroup : AppApiGroupWrapper
{
    internal JobDefinitionsGroup(AppApiGroup source, JobDefinitionsGroupBuilder builder) : base(source)
    {
        GetJobDefinitions = builder.GetJobDefinitions.Build();
        GetRecentTriggeredJobs = builder.GetRecentTriggeredJobs.Build();
        Index = builder.Index.Build();
        Configure();
    }

    partial void Configure();
    public AppApiAction<EmptyRequest, JobDefinitionModel[]> GetJobDefinitions { get; }
    public AppApiAction<GetRecentTriggeredJobsByDefinitionRequest, JobSummaryModel[]> GetRecentTriggeredJobs { get; }
    public AppApiAction<EmptyRequest, WebViewResult> Index { get; }
}