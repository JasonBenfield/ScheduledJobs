namespace XTI_ScheduledJobsWebAppApi.JobInquiry;

public sealed class JobInquiryGroup : AppApiGroupWrapper
{
    public JobInquiryGroup(AppApiGroup source, IServiceProvider sp)
        : base(source)
    {
        var actions = new AppApiActionFactory(source);
        FailedJobs = source.AddAction
        (
            actions.Action(nameof(FailedJobs), () => sp.GetRequiredService<FailedJobsView>())
        );
        GetFailedJobs = source.AddAction
        (
            actions.Action(nameof(GetFailedJobs), () => sp.GetRequiredService<GetFailedJobsAction>())
        );
    }

    public AppApiAction<EmptyRequest, WebViewResult> FailedJobs { get; }
    public AppApiAction<EmptyRequest, JobSummaryModel[]> GetFailedJobs { get; }
}