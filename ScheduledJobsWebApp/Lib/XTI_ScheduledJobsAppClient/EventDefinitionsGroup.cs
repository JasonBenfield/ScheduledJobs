// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class EventDefinitionsGroup : AppClientGroup
{
    public EventDefinitionsGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl, AppClientOptions options) : base(httpClientFactory, xtiTokenAccessor, clientUrl, options, "EventDefinitions")
    {
        Actions = new EventDefinitionsGroupActions(Index: CreateGetAction<EmptyRequest>("Index"), GetEventDefinitions: CreatePostAction<EmptyRequest, EventDefinitionModel[]>("GetEventDefinitions"), GetRecentNotifications: CreatePostAction<GetRecentEventNotificationsByEventDefinitionRequest, EventSummaryModel[]>("GetRecentNotifications"));
    }

    public EventDefinitionsGroupActions Actions { get; }

    public Task<EventDefinitionModel[]> GetEventDefinitions(CancellationToken ct = default) => Actions.GetEventDefinitions.Post("", new EmptyRequest(), ct);
    public Task<EventSummaryModel[]> GetRecentNotifications(GetRecentEventNotificationsByEventDefinitionRequest model, CancellationToken ct = default) => Actions.GetRecentNotifications.Post("", model, ct);
    public sealed record EventDefinitionsGroupActions(AppClientGetAction<EmptyRequest> Index, AppClientPostAction<EmptyRequest, EventDefinitionModel[]> GetEventDefinitions, AppClientPostAction<GetRecentEventNotificationsByEventDefinitionRequest, EventSummaryModel[]> GetRecentNotifications);
}