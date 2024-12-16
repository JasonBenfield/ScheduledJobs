// Generated Code
namespace XTI_ScheduledJobsWebAppApi;
public sealed class ScheduledJobsAppApiFactory : AppApiFactory
{
    private readonly IServiceProvider sp;
    public ScheduledJobsAppApiFactory(IServiceProvider sp)
    {
        this.sp = sp;
    }

    public new ScheduledJobsAppApi Create(IAppApiUser user) => (ScheduledJobsAppApi)base.Create(user);
    public new ScheduledJobsAppApi CreateForSuperUser() => (ScheduledJobsAppApi)base.CreateForSuperUser();
    protected override IAppApi _Create(IAppApiUser user) => new ScheduledJobsAppApiBuilder(sp, user).Build();
}