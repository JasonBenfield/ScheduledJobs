namespace XTI_ScheduledJobsWebAppApi.JobInquiry;

internal sealed class FailedJobsView : AppAction<EmptyRequest, WebViewResult>
{
    private readonly WebViewResultFactory viewFactory;

    public FailedJobsView(WebViewResultFactory viewFactory)
    {
        this.viewFactory = viewFactory;
    }

    public Task<WebViewResult> Execute(EmptyRequest model, CancellationToken stoppingToken) =>
        Task.FromResult(viewFactory.Default("failedJobs", "Failed Jobs"));
}