namespace XTI_ScheduledJobsWebAppApi.Home;

public sealed class HomeGroup : AppApiGroupWrapper
{
    public HomeGroup(AppApiGroup source, IServiceProvider sp)
        : base(source)
    {
        var actions = new WebAppApiActionFactory(source);
        Index = source.AddAction(actions.View(nameof(Index), () => sp.GetRequiredService<IndexAction>()));
    }

    public AppApiAction<EmptyRequest, WebViewResult> Index { get; }
}