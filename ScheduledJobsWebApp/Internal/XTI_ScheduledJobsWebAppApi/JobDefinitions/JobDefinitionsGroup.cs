namespace XTI_ScheduledJobsWebAppApi.JobDefinitions;

public sealed class JobDefinitionsGroup : AppApiGroupWrapper
{
    public JobDefinitionsGroup(AppApiGroup source, IServiceProvider sp)
        : base(source)
    {
        Index = source.AddAction<EmptyRequest, WebViewResult>()
            .Named(nameof(Index))
            .WithExecution<IndexView>()
            .Build();
        GetJobDefinitions = source.AddAction<EmptyRequest, JobDefinitionModel[]>()
            .Named(nameof(GetJobDefinitions))
            .WithExecution<GetJobDefinitionsAction>()
            .Build();
        GetRecentTriggeredJobs = source.AddAction<GetRecentTriggeredJobsByDefinitionRequest, JobSummaryModel[]>()
            .Named(nameof(GetRecentTriggeredJobs))
            .WithExecution<GetRecentTriggeredJobsAction>()
            .Build();
    }

    public AppApiAction<EmptyRequest, WebViewResult> Index { get; }
    public AppApiAction<EmptyRequest, JobDefinitionModel[]> GetJobDefinitions { get; }
    public AppApiAction<GetRecentTriggeredJobsByDefinitionRequest, JobSummaryModel[]> GetRecentTriggeredJobs { get; }
}