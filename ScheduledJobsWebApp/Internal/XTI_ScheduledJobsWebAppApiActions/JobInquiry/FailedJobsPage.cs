namespace XTI_ScheduledJobsWebAppApiActions.JobInquiry;

public sealed class FailedJobsPage : AppAction<EmptyRequest, WebViewResult>
{
    private readonly WebViewResultFactory viewFactory;

    public FailedJobsPage(WebViewResultFactory viewFactory)
    {
        this.viewFactory = viewFactory;
    }

    public Task<WebViewResult> Execute(EmptyRequest model, CancellationToken stoppingToken) =>
        Task.FromResult(viewFactory.Default("failedJobs", "Failed Jobs"));
}