// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class JobInquiryGroup : AppClientGroup
{
    public JobInquiryGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl) : base(httpClientFactory, xtiTokenAccessor, clientUrl, "JobInquiry")
    {
        Actions = new JobInquiryActions(clientUrl);
    }

    public JobInquiryActions Actions { get; }

    public Task<JobSummaryModel[]> GetFailedJobs() => Post<JobSummaryModel[], EmptyRequest>("GetFailedJobs", "", new EmptyRequest());
    public Task<JobSummaryModel[]> GetRecentJobs() => Post<JobSummaryModel[], EmptyRequest>("GetRecentJobs", "", new EmptyRequest());
    public Task<TriggeredJobDetailModel> GetJobDetail(GetJobDetailRequest model) => Post<TriggeredJobDetailModel, GetJobDetailRequest>("GetJobDetail", "", model);
}