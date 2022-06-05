namespace XTI_ScheduledJobsWebAppApi.EventInquiry;

public sealed class NotificationsView : AppAction<EmptyRequest, WebViewResult>
{
    private readonly WebViewResultFactory viewFactory;

    public NotificationsView(WebViewResultFactory viewFactory)
    {
        this.viewFactory = viewFactory;
    }

    public Task<WebViewResult> Execute(EmptyRequest model, CancellationToken ct) =>
        Task.FromResult(viewFactory.Default("eventNotifications", "Recent Events"));
}