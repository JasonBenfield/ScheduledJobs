namespace XTI_ScheduledJobsWebAppApi.EventDefinitions;

public sealed class EventDefinitionsGroup : AppApiGroupWrapper
{
    public EventDefinitionsGroup(AppApiGroup source, IServiceProvider sp)
        : base(source)
    {
        var actions = new AppApiActionFactory(source);
        Index = source.AddAction
        (
            actions.Action(nameof(Index), () => sp.GetRequiredService<IndexView>())
        );
        GetEventDefinitions = source.AddAction
        (
            actions.Action(nameof(GetEventDefinitions), () => sp.GetRequiredService<GetEventDefinitionsAction>())
        );
        GetRecentNotifications = source.AddAction
        (
            actions.Action(nameof(GetRecentNotificationsAction), () => sp.GetRequiredService<GetRecentNotificationsAction>())
        );
    }

    public AppApiAction<EmptyRequest, WebViewResult> Index { get; }
    public AppApiAction<EmptyRequest, EventDefinitionModel[]> GetEventDefinitions { get; }
    public AppApiAction<GetRecentEventNotificationsByEventDefinitionRequest, EventSummaryModel[]> GetRecentNotifications { get; }
}