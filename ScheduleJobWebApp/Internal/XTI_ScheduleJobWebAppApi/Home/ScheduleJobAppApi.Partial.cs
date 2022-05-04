using XTI_ScheduleJobWebAppApi.Home;

namespace XTI_ScheduleJobWebAppApi;

partial class ScheduleJobAppApi
{
    private HomeGroup? home;

    public HomeGroup Home { get => home ?? throw new ArgumentNullException(nameof(home)); }

    partial void createHomeGroup(IServiceProvider sp)
    {
        home = new HomeGroup
        (
            source.AddGroup(nameof(Home), ResourceAccess.AllowAuthenticated()),
            sp
        );
    }
}