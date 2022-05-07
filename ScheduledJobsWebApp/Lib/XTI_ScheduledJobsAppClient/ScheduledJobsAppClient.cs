// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class ScheduledJobsAppClient : AppClient
{
    public ScheduledJobsAppClient(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl, ScheduledJobsAppClientVersion version) : base(httpClientFactory, xtiTokenAccessor, clientUrl, "ScheduledJobs", version.Value)
    {
        User = GetGroup((_clientFactory, _tokenAccessor, _url) => new UserGroup(_clientFactory, _tokenAccessor, _url));
        UserCache = GetGroup((_clientFactory, _tokenAccessor, _url) => new UserCacheGroup(_clientFactory, _tokenAccessor, _url));
        Home = GetGroup((_clientFactory, _tokenAccessor, _url) => new HomeGroup(_clientFactory, _tokenAccessor, _url));
    }

    public ScheduledJobsRoleNames RoleNames { get; } = ScheduledJobsRoleNames.Instance;
    public string AppName { get; } = "ScheduledJobs";
    public UserGroup User { get; }

    public UserCacheGroup UserCache { get; }

    public HomeGroup Home { get; }
}