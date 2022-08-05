namespace XTI_ScheduledJobsWebAppApi.JobInquiry;

internal sealed class RecentJobsView : AppAction<EmptyRequest, WebViewResult>
{
    private readonly WebViewResultFactory viewFactory;

    public RecentJobsView(WebViewResultFactory viewFactory)
    {
        this.viewFactory = viewFactory;
    }

    public Task<WebViewResult> Execute(EmptyRequest model, CancellationToken stoppingToken) =>
        Task.FromResult(viewFactory.Default("recentJobs", "Jobs"));
}