namespace XTI_ScheduledJobsWebAppApiActions.EventInquiry;

public sealed class NotificationDetailPage : AppAction<GetNotificationDetailRequest, WebViewResult>
{
    private readonly WebViewResultFactory viewFactory;

    public NotificationDetailPage(WebViewResultFactory viewFactory)
    {
        this.viewFactory = viewFactory;
    }

    public Task<WebViewResult> Execute(GetNotificationDetailRequest model, CancellationToken stoppingToken) =>
        Task.FromResult(viewFactory.Default("eventNotificationDetail", "Event Detail"));
}
