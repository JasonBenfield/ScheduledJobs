using XTI_ScheduledJobsWebAppApiActions.EventDefinitions;

// Generated Code
#nullable enable
namespace XTI_ScheduledJobsWebAppApi.EventDefinitions;
public sealed partial class EventDefinitionsGroup : AppApiGroupWrapper
{
    internal EventDefinitionsGroup(AppApiGroup source, EventDefinitionsGroupBuilder builder) : base(source)
    {
        GetEventDefinitions = builder.GetEventDefinitions.Build();
        GetRecentNotifications = builder.GetRecentNotifications.Build();
        Index = builder.Index.Build();
        Configure();
    }

    partial void Configure();
    public AppApiAction<EmptyRequest, EventDefinitionModel[]> GetEventDefinitions { get; }
    public AppApiAction<GetRecentEventNotificationsByEventDefinitionRequest, EventSummaryModel[]> GetRecentNotifications { get; }
    public AppApiAction<EmptyRequest, WebViewResult> Index { get; }
}