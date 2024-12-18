using XTI_ScheduledJobsServiceAppApi.Jobs;

// Generated Code
#nullable enable
namespace XTI_ScheduledJobsServiceAppApi;
public sealed partial class ScheduledJobsAppApiBuilder
{
    private readonly AppApi source;
    private readonly IServiceProvider sp;
    public ScheduledJobsAppApiBuilder(IServiceProvider sp, IAppApiUser user)
    {
        source = new AppApi(sp, ScheduledJobsAppKey.Value, user);
        this.sp = sp;
        Jobs = new JobsGroupBuilder(source.AddGroup("Jobs"));
        Configure();
    }

    partial void Configure();
    public JobsGroupBuilder Jobs { get; }

    public ScheduledJobsAppApi Build() => new ScheduledJobsAppApi(source, this);
}