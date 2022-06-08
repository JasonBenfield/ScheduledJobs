// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class JobDefinitionsActions
{
    internal JobDefinitionsActions(AppClientUrl appClientUrl)
    {
        Index = new AppClientGetAction<EmptyRequest>(appClientUrl, "Index");
    }

    public AppClientGetAction<EmptyRequest> Index { get; }
}