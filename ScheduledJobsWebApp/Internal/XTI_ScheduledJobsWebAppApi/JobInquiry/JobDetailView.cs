namespace XTI_ScheduledJobsWebAppApi.JobInquiry;

internal sealed class JobDetailView : AppAction<GetJobDetailRequest, WebViewResult>
{
    private readonly WebViewResultFactory viewFactory;

    public JobDetailView(WebViewResultFactory viewFactory)
    {
        this.viewFactory = viewFactory;
    }

    public Task<WebViewResult> Execute(GetJobDetailRequest model, CancellationToken stoppingToken) =>
        Task.FromResult(viewFactory.Default("jobDetail", "Job Detail"));
}
