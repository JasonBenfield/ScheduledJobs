namespace XTI_ScheduledJobsWebAppApi.Recurring;

public sealed class RecurringGroup : AppApiGroupWrapper
{
    public RecurringGroup(AppApiGroup source, IServiceProvider sp)
        : base(source)
    {
        TimeoutTasks = source.AddAction
        (
            nameof(TimeoutTasks), () => sp.GetRequiredService<TimeoutTasksAction>()
        );
        PurgeJobsAndEvents = source.AddAction
        (
            nameof(PurgeJobsAndEvents), () => sp.GetRequiredService<PurgeJobsAndEventsAction>()
        );
    }

    public AppApiAction<EmptyRequest, EmptyActionResult> TimeoutTasks { get; }
    public AppApiAction<EmptyRequest, EmptyActionResult> PurgeJobsAndEvents { get; }
}