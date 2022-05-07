namespace XTI_ScheduledJobsWebAppApi.Home;

public sealed class IndexAction : AppAction<EmptyRequest, WebViewResult>
{
    private readonly IPageContext pageContext;

    public IndexAction(IPageContext pageContext)
    {
        this.pageContext = pageContext;
    }

    public Task<WebViewResult> Execute(EmptyRequest model)
    {
        var action = new TitledViewAppAction<EmptyRequest>(pageContext, "Index", "ScheduledJobs");
        return action.Execute(model);
    }
}