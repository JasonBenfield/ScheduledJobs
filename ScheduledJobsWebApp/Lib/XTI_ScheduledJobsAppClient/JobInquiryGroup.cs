// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class JobInquiryGroup : AppClientGroup
{
    public JobInquiryGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl, AppClientOptions options) : base(httpClientFactory, xtiTokenAccessor, clientUrl, options, "JobInquiry")
    {
        Actions = new JobInquiryGroupActions(FailedJobs: CreateGetAction<EmptyRequest>("FailedJobs"), GetFailedJobs: CreatePostAction<EmptyRequest, JobSummaryModel[]>("GetFailedJobs"), RecentJobs: CreateGetAction<EmptyRequest>("RecentJobs"), GetRecentJobs: CreatePostAction<EmptyRequest, JobSummaryModel[]>("GetRecentJobs"), JobDetail: CreateGetAction<GetJobDetailRequest>("JobDetail"), GetJobDetail: CreatePostAction<GetJobDetailRequest, TriggeredJobDetailModel>("GetJobDetail"));
    }

    public JobInquiryGroupActions Actions { get; }

    public Task<JobSummaryModel[]> GetFailedJobs() => Actions.GetFailedJobs.Post("", new EmptyRequest());
    public Task<JobSummaryModel[]> GetRecentJobs() => Actions.GetRecentJobs.Post("", new EmptyRequest());
    public Task<TriggeredJobDetailModel> GetJobDetail(GetJobDetailRequest model) => Actions.GetJobDetail.Post("", model);
    public sealed record JobInquiryGroupActions(AppClientGetAction<EmptyRequest> FailedJobs, AppClientPostAction<EmptyRequest, JobSummaryModel[]> GetFailedJobs, AppClientGetAction<EmptyRequest> RecentJobs, AppClientPostAction<EmptyRequest, JobSummaryModel[]> GetRecentJobs, AppClientGetAction<GetJobDetailRequest> JobDetail, AppClientPostAction<GetJobDetailRequest, TriggeredJobDetailModel> GetJobDetail);
}