namespace XTI_ScheduledJobsServiceAppApi;

public sealed partial class ScheduledJobsAppApi : AppApiWrapper
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
                    .WithAllowed(AppRoleName.Admin),
                ""
            )
        )
    {
        createJobsGroup(sp);
    }

    partial void createJobsGroup(IServiceProvider sp);
}