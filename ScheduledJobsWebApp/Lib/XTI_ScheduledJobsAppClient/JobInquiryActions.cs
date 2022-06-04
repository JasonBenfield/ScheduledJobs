// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class JobInquiryActions
{
    internal JobInquiryActions(AppClientUrl appClientUrl)
    {
        FailedJobs = new AppClientGetAction<EmptyRequest>(appClientUrl, "FailedJobs");
    }

    public AppClientGetAction<EmptyRequest> FailedJobs { get; }
}