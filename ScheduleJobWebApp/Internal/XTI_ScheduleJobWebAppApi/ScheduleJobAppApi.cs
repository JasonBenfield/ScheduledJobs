namespace XTI_ScheduleJobWebAppApi;

public sealed partial class ScheduleJobAppApi : WebAppApiWrapper
{
    public ScheduleJobAppApi
    (
        IAppApiUser user,
        IServiceProvider sp
    )
        : base
        (
            new AppApi
            (
                ScheduleJobInfo.AppKey,
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