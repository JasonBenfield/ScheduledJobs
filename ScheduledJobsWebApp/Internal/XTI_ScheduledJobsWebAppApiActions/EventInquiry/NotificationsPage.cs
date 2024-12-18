namespace XTI_ScheduledJobsWebAppApiActions.EventInquiry;

public sealed class NotificationsPage : AppAction<EmptyRequest, WebViewResult>
{
    private readonly WebViewResultFactory viewFactory;

    public NotificationsPage(WebViewResultFactory viewFactory)
    {
        this.viewFactory = viewFactory;
    }

    public Task<WebViewResult> Execute(EmptyRequest model, CancellationToken ct) =>
        Task.FromResult(viewFactory.Default("eventNotifications", "Recent Events"));
}