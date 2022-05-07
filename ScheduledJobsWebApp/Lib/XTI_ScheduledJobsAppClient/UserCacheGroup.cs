// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class UserCacheGroup : AppClientGroup
{
    public UserCacheGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl) : base(httpClientFactory, xtiTokenAccessor, clientUrl, "UserCache")
    {
    }

    public Task<EmptyActionResult> ClearCache(string model) => Post<EmptyActionResult, string>("ClearCache", "", model);
}