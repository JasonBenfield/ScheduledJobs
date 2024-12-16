namespace XTI_ScheduledJobsWebAppApiActions.JobInquiry;

public sealed class RecentJobsPage : AppAction<EmptyRequest, WebViewResult>
{
    private readonly WebViewResultFactory viewFactory;

    public RecentJobsPage(WebViewResultFactory viewFactory)
    {
        this.viewFactory = viewFactory;
    }

    public Task<WebViewResult> Execute(EmptyRequest model, CancellationToken stoppingToken) =>
        Task.FromResult(viewFactory.Default("recentJobs", "Jobs"));
}