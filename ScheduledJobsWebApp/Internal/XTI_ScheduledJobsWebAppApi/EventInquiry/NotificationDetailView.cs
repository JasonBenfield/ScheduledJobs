namespace XTI_ScheduledJobsWebAppApi.EventInquiry;

internal sealed class NotificationDetailView : AppAction<GetNotificationDetailRequest, WebViewResult>
{
    private readonly WebViewResultFactory viewFactory;

    public NotificationDetailView(WebViewResultFactory viewFactory)
    {
        this.viewFactory = viewFactory;
    }

    public Task<WebViewResult> Execute(GetNotificationDetailRequest model, CancellationToken stoppingToken) =>
        Task.FromResult(viewFactory.Default("eventNotificationDetail", "Event Detail"));
}
