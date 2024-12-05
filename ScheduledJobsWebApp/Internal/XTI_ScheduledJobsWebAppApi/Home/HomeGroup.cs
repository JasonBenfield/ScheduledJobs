namespace XTI_ScheduledJobsWebAppApi.Home;

public sealed class HomeGroup : AppApiGroupWrapper
{
    public HomeGroup(AppApiGroup source, IServiceProvider sp)
        : base(source)
    {
        Index = source.AddAction<EmptyRequest, WebViewResult>()
            .Named(nameof(Index))
            .WithExecution<IndexAction>()
            .Build();
    }

    public AppApiAction<EmptyRequest, WebViewResult> Index { get; }
}