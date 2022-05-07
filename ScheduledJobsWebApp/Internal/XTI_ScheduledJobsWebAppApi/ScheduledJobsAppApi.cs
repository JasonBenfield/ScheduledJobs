namespace XTI_ScheduledJobsWebAppApi;

public sealed partial class ScheduledJobsAppApi : WebAppApiWrapper
{
    public ScheduledJobsAppApi
    (
        IAppApiUser user,
        IServiceProvider sp
    )
        : base
        (
            new AppApi
            (
                ScheduledJobsInfo.AppKey,
                user,
                ResourceAccess.AllowAuthenticated()
                    .WithAllowed(AppRoleName.Admin)
            ),
            sp
        )
    {
        createHomeGroup(sp);
    }

    partial void createHomeGroup(IServiceProvider sp);
}