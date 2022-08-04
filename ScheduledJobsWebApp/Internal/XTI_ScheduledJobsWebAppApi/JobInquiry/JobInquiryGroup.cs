namespace XTI_ScheduledJobsWebAppApi.JobInquiry;

public sealed class JobInquiryGroup : AppApiGroupWrapper
{
    public JobInquiryGroup(AppApiGroup source, IServiceProvider sp)
        : base(source)
    {
        FailedJobs = source.AddAction
        (
            nameof(FailedJobs), () => sp.GetRequiredService<FailedJobsView>()
        );
        GetFailedJobs = source.AddAction
        (
            nameof(GetFailedJobs), () => sp.GetRequiredService<GetFailedJobsAction>()
        );
        RecentJobs = source.AddAction
        (
            nameof(RecentJobs), () => sp.GetRequiredService<RecentJobsView>()
        );
        GetRecentJobs = source.AddAction
        (
            nameof(GetRecentJobs), () => sp.GetRequiredService<GetRecentJobsAction>()
        );
        JobDetail = source.AddAction
        (
            nameof(JobDetail), () => sp.GetRequiredService<JobDetailView>()
        );
        GetJobDetail = source.AddAction
        (
            nameof(GetJobDetail), () => sp.GetRequiredService<GetJobDetailAction>()
        );
    }

    public AppApiAction<EmptyRequest, WebViewResult> FailedJobs { get; }
    public AppApiAction<EmptyRequest, JobSummaryModel[]> GetFailedJobs { get; }
    public AppApiAction<EmptyRequest, WebViewResult> RecentJobs { get; }
    public AppApiAction<EmptyRequest, JobSummaryModel[]> GetRecentJobs { get; }
    public AppApiAction<GetJobDetailRequest, WebViewResult> JobDetail { get; }
    public AppApiAction<GetJobDetailRequest, TriggeredJobDetailModel> GetJobDetail { get; }
}