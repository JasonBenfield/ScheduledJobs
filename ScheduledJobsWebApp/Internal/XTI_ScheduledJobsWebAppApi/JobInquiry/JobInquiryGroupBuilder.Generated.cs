using XTI_ScheduledJobsWebAppApiActions.JobInquiry;

// Generated Code
#nullable enable
namespace XTI_ScheduledJobsWebAppApi.JobInquiry;
public sealed partial class JobInquiryGroupBuilder
{
    private readonly AppApiGroup source;
    internal JobInquiryGroupBuilder(AppApiGroup source)
    {
        this.source = source;
        FailedJobs = source.AddAction<EmptyRequest, WebViewResult>("FailedJobs").WithExecution<FailedJobsPage>();
        GetFailedJobs = source.AddAction<EmptyRequest, JobSummaryModel[]>("GetFailedJobs").WithExecution<GetFailedJobsAction>();
        GetJobDetail = source.AddAction<GetJobDetailRequest, TriggeredJobDetailModel>("GetJobDetail").WithExecution<GetJobDetailAction>();
        GetRecentJobs = source.AddAction<EmptyRequest, JobSummaryModel[]>("GetRecentJobs").WithExecution<GetRecentJobsAction>();
        JobDetail = source.AddAction<GetJobDetailRequest, WebViewResult>("JobDetail").WithExecution<JobDetailPage>();
        RecentJobs = source.AddAction<EmptyRequest, WebViewResult>("RecentJobs").WithExecution<RecentJobsPage>();
        Configure();
    }

    partial void Configure();
    public AppApiActionBuilder<EmptyRequest, WebViewResult> FailedJobs { get; }
    public AppApiActionBuilder<EmptyRequest, JobSummaryModel[]> GetFailedJobs { get; }
    public AppApiActionBuilder<GetJobDetailRequest, TriggeredJobDetailModel> GetJobDetail { get; }
    public AppApiActionBuilder<EmptyRequest, JobSummaryModel[]> GetRecentJobs { get; }
    public AppApiActionBuilder<GetJobDetailRequest, WebViewResult> JobDetail { get; }
    public AppApiActionBuilder<EmptyRequest, WebViewResult> RecentJobs { get; }

    public JobInquiryGroup Build() => new JobInquiryGroup(source, this);
}