// Generated Code
namespace XTI_ScheduledJobsAppClient;
public sealed partial class UserActions
{
    internal UserActions(AppClientUrl appClientUrl)
    {
        Index = new AppClientGetAction<UserStartRequest>(appClientUrl, "Index");
        AccessDenied = new AppClientGetAction<EmptyRequest>(appClientUrl, "AccessDenied");
        Error = new AppClientGetAction<EmptyRequest>(appClientUrl, "Error");
        Logout = new AppClientGetAction<LogoutRequest>(appClientUrl, "Logout");
    }

    public AppClientGetAction<UserStartRequest> Index { get; }

    public AppClientGetAction<EmptyRequest> AccessDenied { get; }

    public AppClientGetAction<EmptyRequest> Error { get; }

    public AppClientGetAction<LogoutRequest> Logout { get; }
}