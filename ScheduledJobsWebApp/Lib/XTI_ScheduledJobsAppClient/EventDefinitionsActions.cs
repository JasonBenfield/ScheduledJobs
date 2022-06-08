// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class EventDefinitionsActions
{
    internal EventDefinitionsActions(AppClientUrl appClientUrl)
    {
        Index = new AppClientGetAction<EmptyRequest>(appClientUrl, "Index");
    }

    public AppClientGetAction<EmptyRequest> Index { get; }
}