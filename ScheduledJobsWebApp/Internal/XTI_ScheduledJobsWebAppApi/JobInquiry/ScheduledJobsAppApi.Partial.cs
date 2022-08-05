using XTI_ScheduledJobsWebAppApi.JobInquiry;

namespace XTI_ScheduledJobsWebAppApi;

partial class ScheduledJobsAppApi
{
    private JobInquiryGroup? _JobInquiry;

    public JobInquiryGroup JobInquiry { get => _JobInquiry ?? throw new ArgumentNullException(nameof(_JobInquiry)); }

    partial void createJobInquiryGroup(IServiceProvider sp)
    {
        _JobInquiry = new JobInquiryGroup
        (
            source.AddGroup(nameof(JobInquiry)),
            sp
        );
    }
}