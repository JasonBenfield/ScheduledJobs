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
        JobDetail = source.AddAction
        (
            actions.Action(nameof(JobDetail), () => sp.GetRequiredService<JobDetailView>())
        );
        GetJobDetail = source.AddAction
        (
            actions.Action(nameof(GetJobDetail), () => sp.GetRequiredService<GetJobDetailAction>())
        );
    }

    public AppApiAction<EmptyRequest, WebViewResult> FailedJobs { get; }
    public AppApiAction<EmptyRequest, JobSummaryModel[]> GetFailedJobs { get; }
    public AppApiAction<GetJobDetailRequest, WebViewResult> JobDetail { get; }
    public AppApiAction<GetJobDetailRequest, TriggeredJobDetailModel> GetJobDetail { get; }
}