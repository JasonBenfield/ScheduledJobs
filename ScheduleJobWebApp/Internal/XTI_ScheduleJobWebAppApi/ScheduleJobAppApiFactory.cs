namespace XTI_ScheduleJobWebAppApi;

public sealed class ScheduleJobAppApiFactory : AppApiFactory
{
    private readonly IServiceProvider sp;

    public ScheduleJobAppApiFactory(IServiceProvider sp)
    {
        this.sp = sp;
    }

    public new ScheduleJobAppApi Create(IAppApiUser user) => (ScheduleJobAppApi)base.Create(user);
    public new ScheduleJobAppApi CreateForSuperUser() => (ScheduleJobAppApi)base.CreateForSuperUser();

    protected override IAppApi _Create(IAppApiUser user) => new ScheduleJobAppApi(user, sp);
}