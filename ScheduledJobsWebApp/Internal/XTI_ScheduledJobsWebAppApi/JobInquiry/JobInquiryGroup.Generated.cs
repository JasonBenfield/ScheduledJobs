using XTI_ScheduledJobsWebAppApiActions.JobInquiry;

// Generated Code
#nullable enable
namespace XTI_ScheduledJobsWebAppApi.JobInquiry;
public sealed partial class JobInquiryGroup : AppApiGroupWrapper
{
    internal JobInquiryGroup(AppApiGroup source, JobInquiryGroupBuilder builder) : base(source)
    {
        FailedJobs = builder.FailedJobs.Build();
        GetFailedJobs = builder.GetFailedJobs.Build();
        GetJobDetail = builder.GetJobDetail.Build();
        GetRecentJobs = builder.GetRecentJobs.Build();
        JobDetail = builder.JobDetail.Build();
        RecentJobs = builder.RecentJobs.Build();
        Configure();
    }

    partial void Configure();
    public AppApiAction<EmptyRequest, WebViewResult> FailedJobs { get; }
    public AppApiAction<EmptyRequest, JobSummaryModel[]> GetFailedJobs { get; }
    public AppApiAction<GetJobDetailRequest, TriggeredJobDetailModel> GetJobDetail { get; }
    public AppApiAction<EmptyRequest, JobSummaryModel[]> GetRecentJobs { get; }
    public AppApiAction<GetJobDetailRequest, WebViewResult> JobDetail { get; }
    public AppApiAction<EmptyRequest, WebViewResult> RecentJobs { get; }
}