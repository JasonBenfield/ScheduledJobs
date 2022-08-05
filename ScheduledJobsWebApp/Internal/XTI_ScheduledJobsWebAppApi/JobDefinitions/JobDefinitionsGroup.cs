namespace XTI_ScheduledJobsWebAppApi.JobDefinitions;

public sealed class JobDefinitionsGroup : AppApiGroupWrapper
{
    public JobDefinitionsGroup(AppApiGroup source, IServiceProvider sp)
        : base(source)
    {
        Index = source.AddAction(nameof(Index), () => sp.GetRequiredService<IndexView>());
        GetJobDefinitions = source.AddAction
        (
            nameof(GetJobDefinitions), () => sp.GetRequiredService<GetJobDefinitionsAction>()
        );
        GetRecentTriggeredJobs = source.AddAction
        (
            nameof(GetRecentTriggeredJobs), () => sp.GetRequiredService<GetRecentTriggeredJobsAction>()
        );
    }

    public AppApiAction<EmptyRequest, WebViewResult> Index { get; }
    public AppApiAction<EmptyRequest, JobDefinitionModel[]> GetJobDefinitions { get; }
    public AppApiAction<GetRecentTriggeredJobsByDefinitionRequest, JobSummaryModel[]> GetRecentTriggeredJobs { get; }
}