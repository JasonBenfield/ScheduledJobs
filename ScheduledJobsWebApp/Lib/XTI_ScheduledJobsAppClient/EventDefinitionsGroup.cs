// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class EventDefinitionsGroup : AppClientGroup
{
    public EventDefinitionsGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl) : base(httpClientFactory, xtiTokenAccessor, clientUrl, "EventDefinitions")
    {
        Actions = new EventDefinitionsActions(clientUrl);
    }

    public EventDefinitionsActions Actions { get; }

    public Task<EventDefinitionModel[]> GetEventDefinitions() => Post<EventDefinitionModel[], EmptyRequest>("GetEventDefinitions", "", new EmptyRequest());
    public Task<EventSummaryModel[]> GetRecentNotificationsAction(GetRecentEventNotificationsByEventDefinitionRequest model) => Post<EventSummaryModel[], GetRecentEventNotificationsByEventDefinitionRequest>("GetRecentNotificationsAction", "", model);
}