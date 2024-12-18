using XTI_ScheduledJobsServiceAppApi.Jobs;

// Generated Code
#nullable enable
namespace XTI_ScheduledJobsServiceAppApi;
public sealed partial class ScheduledJobsAppApi : AppApiWrapper
{
    internal ScheduledJobsAppApi(AppApi source, ScheduledJobsAppApiBuilder builder) : base(source)
    {
        Jobs = builder.Jobs.Build();
        Configure();
    }

    partial void Configure();
    public JobsGroup Jobs { get; }
}