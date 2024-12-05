namespace XTI_ScheduledJobsWebAppApi.EventDefinitions;

public sealed class EventDefinitionsGroup : AppApiGroupWrapper
{
    public EventDefinitionsGroup(AppApiGroup source, IServiceProvider sp)
        : base(source)
    {
        Index = source.AddAction<EmptyRequest, WebViewResult>()
            .Named(nameof(Index))
            .WithExecution<IndexView>()
            .Build();
        GetEventDefinitions = source.AddAction<EmptyRequest, EventDefinitionModel[]>()
            .Named(nameof(GetEventDefinitions))
            .WithExecution<GetEventDefinitionsAction>()
            .Build();
        GetRecentNotifications = source.AddAction<GetRecentEventNotificationsByEventDefinitionRequest, EventSummaryModel[]>()
            .Named(nameof(GetRecentNotifications))
            .WithExecution<GetRecentNotificationsAction>()
            .Build();
    }

    public AppApiAction<EmptyRequest, WebViewResult> Index { get; }
    public AppApiAction<EmptyRequest, EventDefinitionModel[]> GetEventDefinitions { get; }
    public AppApiAction<GetRecentEventNotificationsByEventDefinitionRequest, EventSummaryModel[]> GetRecentNotifications { get; }
}