// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class UserGroup : AppClientGroup
{
    public UserGroup(IHttpClientFactory httpClientFactory, XtiTokenAccessor xtiTokenAccessor, AppClientUrl clientUrl, AppClientOptions options) : base(httpClientFactory, xtiTokenAccessor, clientUrl, options, "User")
    {
        Actions = new UserGroupActions(GetUserAccess: CreatePostAction<ResourcePath[], ResourcePathAccess[]>("GetUserAccess"), AccessDenied: CreateGetAction<EmptyRequest>("AccessDenied"), Error: CreateGetAction<EmptyRequest>("Error"), Logout: CreateGetAction<LogoutRequest>("Logout"));
    }

    public UserGroupActions Actions { get; }

    public Task<ResourcePathAccess[]> GetUserAccess(ResourcePath[] model) => Actions.GetUserAccess.Post("", model);
    public sealed record UserGroupActions(AppClientPostAction<ResourcePath[], ResourcePathAccess[]> GetUserAccess, AppClientGetAction<EmptyRequest> AccessDenied, AppClientGetAction<EmptyRequest> Error, AppClientGetAction<LogoutRequest> Logout);
}