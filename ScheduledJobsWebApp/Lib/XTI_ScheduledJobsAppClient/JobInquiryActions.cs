// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class JobInquiryActions
{
    internal JobInquiryActions(AppClientUrl appClientUrl)
    {
        FailedJobs = new AppClientGetAction<EmptyRequest>(appClientUrl, "FailedJobs");
        RecentJobs = new AppClientGetAction<EmptyRequest>(appClientUrl, "RecentJobs");
        JobDetail = new AppClientGetAction<GetJobDetailRequest>(appClientUrl, "JobDetail");
    }

    public AppClientGetAction<EmptyRequest> FailedJobs { get; }

    public AppClientGetAction<EmptyRequest> RecentJobs { get; }

    public AppClientGetAction<GetJobDetailRequest> JobDetail { get; }
}