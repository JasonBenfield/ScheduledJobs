namespace XTI_ScheduledJobsWebAppApiActions.JobInquiry;

public sealed class JobDetailPage : AppAction<GetJobDetailRequest, WebViewResult>
{
    private readonly WebViewResultFactory viewFactory;

    public JobDetailPage(WebViewResultFactory viewFactory)
    {
        this.viewFactory = viewFactory;
    }

    public Task<WebViewResult> Execute(GetJobDetailRequest model, CancellationToken stoppingToken) =>
        Task.FromResult(viewFactory.Default("jobDetail", "Job Detail"));
}
