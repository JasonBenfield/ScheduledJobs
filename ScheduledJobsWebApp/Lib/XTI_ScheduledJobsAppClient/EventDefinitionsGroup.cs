// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class EventDefinitionsGroup : AppClientGroup
{
    public EventDefinitionsGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl, AppClientOptions options) : base(httpClientFactory, xtiTokenAccessor, clientUrl, options, "EventDefinitions")
    {
        Actions = new EventDefinitionsGroupActions(GetEventDefinitions: CreatePostAction<EmptyRequest, EventDefinitionModel[]>("GetEventDefinitions"), GetRecentNotifications: CreatePostAction<GetRecentEventNotificationsByEventDefinitionRequest, EventSummaryModel[]>("GetRecentNotifications"), Index: CreateGetAction<EmptyRequest>("Index"));
    }

    public EventDefinitionsGroupActions Actions { get; }

    public Task<EventDefinitionModel[]> GetEventDefinitions(CancellationToken ct = default) => Actions.GetEventDefinitions.Post("", new EmptyRequest(), ct);
    public Task<EventSummaryModel[]> GetRecentNotifications(GetRecentEventNotificationsByEventDefinitionRequest requestData, CancellationToken ct = default) => Actions.GetRecentNotifications.Post("", requestData, ct);
    public sealed record EventDefinitionsGroupActions(AppClientPostAction<EmptyRequest, EventDefinitionModel[]> GetEventDefinitions, AppClientPostAction<GetRecentEventNotificationsByEventDefinitionRequest, EventSummaryModel[]> GetRecentNotifications, AppClientGetAction<EmptyRequest> Index);
}