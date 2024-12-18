using XTI_ScheduledJobsServiceAppApiActions.Jobs;

// Generated Code
#nullable enable
namespace XTI_ScheduledJobsServiceAppApi.Jobs;
public sealed partial class JobsGroupBuilder
{
    private readonly AppApiGroup source;
    internal JobsGroupBuilder(AppApiGroup source)
    {
        this.source = source;
        AddJobScheduleNotifications = source.AddAction<EmptyRequest, EmptyActionResult>("AddJobScheduleNotifications").WithExecution<AddJobScheduleNotificationsAction>();
        PurgeJobsAndEvents = source.AddAction<EmptyRequest, EmptyActionResult>("PurgeJobsAndEvents").WithExecution<PurgeJobsAndEventsAction>();
        TimeoutJobs = source.AddAction<EmptyRequest, EmptyActionResult>("TimeoutJobs").WithExecution<TimeoutJobsAction>();
        Configure();
    }

    partial void Configure();
    public AppApiActionBuilder<EmptyRequest, EmptyActionResult> AddJobScheduleNotifications { get; }
    public AppApiActionBuilder<EmptyRequest, EmptyActionResult> PurgeJobsAndEvents { get; }
    public AppApiActionBuilder<EmptyRequest, EmptyActionResult> TimeoutJobs { get; }

    public JobsGroup Build() => new JobsGroup(source, this);
}