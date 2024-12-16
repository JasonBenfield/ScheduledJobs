using XTI_ScheduledJobsWebAppApiActions.EventDefinitions;

// Generated Code
#nullable enable
namespace XTI_ScheduledJobsWebAppApi.EventDefinitions;
public sealed partial class EventDefinitionsGroupBuilder
{
    private readonly AppApiGroup source;
    internal EventDefinitionsGroupBuilder(AppApiGroup source)
    {
        this.source = source;
        GetEventDefinitions = source.AddAction<EmptyRequest, EventDefinitionModel[]>("GetEventDefinitions").WithExecution<GetEventDefinitionsAction>();
        GetRecentNotifications = source.AddAction<GetRecentEventNotificationsByEventDefinitionRequest, EventSummaryModel[]>("GetRecentNotifications").WithExecution<GetRecentNotificationsAction>();
        Index = source.AddAction<EmptyRequest, WebViewResult>("Index").WithExecution<IndexPage>();
        Configure();
    }

    partial void Configure();
    public AppApiActionBuilder<EmptyRequest, EventDefinitionModel[]> GetEventDefinitions { get; }
    public AppApiActionBuilder<GetRecentEventNotificationsByEventDefinitionRequest, EventSummaryModel[]> GetRecentNotifications { get; }
    public AppApiActionBuilder<EmptyRequest, WebViewResult> Index { get; }

    public EventDefinitionsGroup Build() => new EventDefinitionsGroup(source, this);
}