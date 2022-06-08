namespace XTI_ScheduledJobsWebAppApi.EventDefinitions;

public sealed class IndexView : AppAction<EmptyRequest, WebViewResult>
{
    private readonly WebViewResultFactory viewFactory;

    public IndexView(WebViewResultFactory viewFactory)
    {
        this.viewFactory = viewFactory;
    }

    public Task<WebViewResult> Execute(EmptyRequest model, CancellationToken ct) =>
        Task.FromResult(viewFactory.Default("eventDefinitions", "Event Definitions"));
}