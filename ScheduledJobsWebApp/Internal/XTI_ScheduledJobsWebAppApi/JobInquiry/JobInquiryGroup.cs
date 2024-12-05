namespace XTI_ScheduledJobsWebAppApi.JobInquiry;

public sealed class JobInquiryGroup : AppApiGroupWrapper
{
    public JobInquiryGroup(AppApiGroup source, IServiceProvider sp)
        : base(source)
    {
        FailedJobs = source.AddAction<EmptyRequest, WebViewResult>()
            .Named(nameof(FailedJobs))
            .WithExecution<FailedJobsView>()
            .Build();
        GetFailedJobs = source.AddAction<EmptyRequest, JobSummaryModel[]>()
            .Named(nameof(GetFailedJobs))
            .WithExecution<GetFailedJobsAction>()
            .Build();
        RecentJobs = source.AddAction<EmptyRequest, WebViewResult>()
            .Named(nameof(RecentJobs))
            .WithExecution<RecentJobsView>()
            .Build();
        GetRecentJobs = source.AddAction<EmptyRequest, JobSummaryModel[]>()
            .Named(nameof(GetRecentJobs))
            .WithExecution<GetRecentJobsAction>()
            .Build();
        JobDetail = source.AddAction<GetJobDetailRequest, WebViewResult>()
            .Named(nameof(JobDetail))
            .WithExecution<JobDetailView>()
            .Build();
        GetJobDetail = source.AddAction<GetJobDetailRequest, TriggeredJobDetailModel>()
            .Named(nameof(GetJobDetail))
            .WithExecution<GetJobDetailAction>()
            .Build();
    }

    public AppApiAction<EmptyRequest, WebViewResult> FailedJobs { get; }
    public AppApiAction<EmptyRequest, JobSummaryModel[]> GetFailedJobs { get; }
    public AppApiAction<EmptyRequest, WebViewResult> RecentJobs { get; }
    public AppApiAction<EmptyRequest, JobSummaryModel[]> GetRecentJobs { get; }
    public AppApiAction<GetJobDetailRequest, WebViewResult> JobDetail { get; }
    public AppApiAction<GetJobDetailRequest, TriggeredJobDetailModel> GetJobDetail { get; }
}