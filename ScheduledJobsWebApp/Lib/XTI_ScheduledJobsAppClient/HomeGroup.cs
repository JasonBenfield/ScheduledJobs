// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class HomeGroup : AppClientGroup
{
    public HomeGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl) : base(httpClientFactory, xtiTokenAccessor, clientUrl, "Home")
    {
        Actions = new HomeActions(clientUrl);
    }

    public HomeActions Actions { get; }
}