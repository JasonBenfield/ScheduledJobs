using XTI_ScheduledJobsWebAppApiActions.Recurring;

// Generated Code
#nullable enable
namespace XTI_ScheduledJobsWebAppApi.Recurring;
public sealed partial class RecurringGroup : AppApiGroupWrapper
{
    internal RecurringGroup(AppApiGroup source, RecurringGroupBuilder builder) : base(source)
    {
        AddJobScheduleNotifications = builder.AddJobScheduleNotifications.Build();
        PurgeJobsAndEvents = builder.PurgeJobsAndEvents.Build();
        TimeoutTasks = builder.TimeoutTasks.Build();
        Configure();
    }

    partial void Configure();
    public AppApiAction<EmptyRequest, EmptyActionResult> AddJobScheduleNotifications { get; }
    public AppApiAction<EmptyRequest, EmptyActionResult> PurgeJobsAndEvents { get; }
    public AppApiAction<EmptyRequest, EmptyActionResult> TimeoutTasks { get; }
}