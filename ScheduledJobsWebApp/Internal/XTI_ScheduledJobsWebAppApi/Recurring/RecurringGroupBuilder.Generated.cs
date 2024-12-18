using XTI_ScheduledJobsWebAppApiActions.Recurring;

// Generated Code
#nullable enable
namespace XTI_ScheduledJobsWebAppApi.Recurring;
public sealed partial class RecurringGroupBuilder
{
    private readonly AppApiGroup source;
    internal RecurringGroupBuilder(AppApiGroup source)
    {
        this.source = source;
        AddJobScheduleNotifications = source.AddAction<EmptyRequest, EmptyActionResult>("AddJobScheduleNotifications").WithExecution<AddJobScheduleNotificationsAction>();
        PurgeJobsAndEvents = source.AddAction<EmptyRequest, EmptyActionResult>("PurgeJobsAndEvents").WithExecution<PurgeJobsAndEventsAction>();
        TimeoutTasks = source.AddAction<EmptyRequest, EmptyActionResult>("TimeoutTasks").WithExecution<TimeoutTasksAction>();
        Configure();
    }

    partial void Configure();
    public AppApiActionBuilder<EmptyRequest, EmptyActionResult> AddJobScheduleNotifications { get; }
    public AppApiActionBuilder<EmptyRequest, EmptyActionResult> PurgeJobsAndEvents { get; }
    public AppApiActionBuilder<EmptyRequest, EmptyActionResult> TimeoutTasks { get; }

    public RecurringGroup Build() => new RecurringGroup(source, this);
}