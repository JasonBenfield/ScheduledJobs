// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class UserGroup : AppClientGroup
{
    public UserGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl) : base(httpClientFactory, xtiTokenAccessor, clientUrl, "User")
    {
        Actions = new UserActions(clientUrl);
    }

    public UserActions Actions { get; }
}