using XTI_ScheduledJobsServiceAppApiActions.Jobs;

// Generated Code
#nullable enable
namespace XTI_ScheduledJobsServiceAppApi.Jobs;
public sealed partial class JobsGroup : AppApiGroupWrapper
{
    internal JobsGroup(AppApiGroup source, JobsGroupBuilder builder) : base(source)
    {
        AddJobScheduleNotifications = builder.AddJobScheduleNotifications.Build();
        PurgeJobsAndEvents = builder.PurgeJobsAndEvents.Build();
        TimeoutJobs = builder.TimeoutJobs.Build();
        Configure();
    }

    partial void Configure();
    public AppApiAction<EmptyRequest, EmptyActionResult> AddJobScheduleNotifications { get; }
    public AppApiAction<EmptyRequest, EmptyActionResult> PurgeJobsAndEvents { get; }
    public AppApiAction<EmptyRequest, EmptyActionResult> TimeoutJobs { get; }
}