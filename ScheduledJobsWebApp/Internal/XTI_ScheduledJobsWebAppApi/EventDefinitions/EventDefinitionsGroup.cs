namespace XTI_ScheduledJobsWebAppApi.EventDefinitions;

public sealed class EventDefinitionsGroup : AppApiGroupWrapper
{
    public EventDefinitionsGroup(AppApiGroup source, IServiceProvider sp)
        : base(source)
    {
        Index = source.AddAction
        (
            nameof(Index), () => sp.GetRequiredService<IndexView>()
        );
        GetEventDefinitions = source.AddAction
        (
            nameof(GetEventDefinitions), () => sp.GetRequiredService<GetEventDefinitionsAction>()
        );
        GetRecentNotifications = source.AddAction
        (
            nameof(GetRecentNotifications), () => sp.GetRequiredService<GetRecentNotificationsAction>()
        );
    }

    public AppApiAction<EmptyRequest, WebViewResult> Index { get; }
    public AppApiAction<EmptyRequest, EventDefinitionModel[]> GetEventDefinitions { get; }
    public AppApiAction<GetRecentEventNotificationsByEventDefinitionRequest, EventSummaryModel[]> GetRecentNotifications { get; }
}