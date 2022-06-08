namespace XTI_ScheduledJobsWebAppApi.JobDefinitions;

public sealed class JobDefinitionsGroup : AppApiGroupWrapper
{
    public JobDefinitionsGroup(AppApiGroup source, IServiceProvider sp)
        : base(source)
    {
        var actions = new AppApiActionFactory(source);
        Index = source.AddAction(actions.Action(nameof(Index), () => sp.GetRequiredService<IndexView>()));
        GetJobDefinitions = source.AddAction
        (
            actions.Action(nameof(GetJobDefinitions), () => sp.GetRequiredService<GetJobDefinitionsAction>())
        );
        GetRecentTriggeredJobs = source.AddAction
        (
            actions.Action(nameof(GetRecentTriggeredJobs), () => sp.GetRequiredService<GetRecentTriggeredJobsAction>())
        );
    }

    public AppApiAction<EmptyRequest, WebViewResult> Index { get; }
    public AppApiAction<EmptyRequest, JobDefinitionModel[]> GetJobDefinitions { get; }
    public AppApiAction<GetRecentTriggeredJobsByDefinitionRequest, JobSummaryModel[]> GetRecentTriggeredJobs { get; }
}