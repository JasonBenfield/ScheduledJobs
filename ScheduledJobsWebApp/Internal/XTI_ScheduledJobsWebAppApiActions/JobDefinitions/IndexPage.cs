namespace XTI_ScheduledJobsWebAppApiActions.JobDefinitions;

public sealed class IndexPage : AppAction<EmptyRequest, WebViewResult>
{
    private readonly WebViewResultFactory viewFactory;

    public IndexPage(WebViewResultFactory viewFactory)
    {
        this.viewFactory = viewFactory;
    }

    public Task<WebViewResult> Execute(EmptyRequest model, CancellationToken ct) =>
        Task.FromResult(viewFactory.Default("jobDefinitions", "Job Definitions"));
}