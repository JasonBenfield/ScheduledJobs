// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class JobInquiryActions
{
    internal JobInquiryActions(AppClientUrl appClientUrl)
    {
        FailedJobs = new AppClientGetAction<EmptyRequest>(appClientUrl, "FailedJobs");
        JobDetail = new AppClientGetAction<GetJobDetailRequest>(appClientUrl, "JobDetail");
    }

    public AppClientGetAction<EmptyRequest> FailedJobs { get; }

    public AppClientGetAction<GetJobDetailRequest> JobDetail { get; }
}