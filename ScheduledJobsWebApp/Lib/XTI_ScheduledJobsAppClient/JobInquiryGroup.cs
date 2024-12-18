// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class JobInquiryGroup : AppClientGroup
{
    public JobInquiryGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl, AppClientOptions options) : base(httpClientFactory, xtiTokenAccessor, clientUrl, options, "JobInquiry")
    {
        Actions = new JobInquiryGroupActions(FailedJobs: CreateGetAction<EmptyRequest>("FailedJobs"), GetFailedJobs: CreatePostAction<EmptyRequest, JobSummaryModel[]>("GetFailedJobs"), GetJobDetail: CreatePostAction<GetJobDetailRequest, TriggeredJobDetailModel>("GetJobDetail"), GetRecentJobs: CreatePostAction<EmptyRequest, JobSummaryModel[]>("GetRecentJobs"), JobDetail: CreateGetAction<GetJobDetailRequest>("JobDetail"), RecentJobs: CreateGetAction<EmptyRequest>("RecentJobs"));
    }

    public JobInquiryGroupActions Actions { get; }

    public Task<JobSummaryModel[]> GetFailedJobs(CancellationToken ct = default) => Actions.GetFailedJobs.Post("", new EmptyRequest(), ct);
    public Task<TriggeredJobDetailModel> GetJobDetail(GetJobDetailRequest requestData, CancellationToken ct = default) => Actions.GetJobDetail.Post("", requestData, ct);
    public Task<JobSummaryModel[]> GetRecentJobs(CancellationToken ct = default) => Actions.GetRecentJobs.Post("", new EmptyRequest(), ct);
    public sealed record JobInquiryGroupActions(AppClientGetAction<EmptyRequest> FailedJobs, AppClientPostAction<EmptyRequest, JobSummaryModel[]> GetFailedJobs, AppClientPostAction<GetJobDetailRequest, TriggeredJobDetailModel> GetJobDetail, AppClientPostAction<EmptyRequest, JobSummaryModel[]> GetRecentJobs, AppClientGetAction<GetJobDetailRequest> JobDetail, AppClientGetAction<EmptyRequest> RecentJobs);
}