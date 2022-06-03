namespace XTI_ScheduledJobsWebAppApi.Recurring;

public sealed class RecurringGroup : AppApiGroupWrapper
{
    public RecurringGroup(AppApiGroup source, IServiceProvider sp)
        : base(source)
    {
        var actions = new AppApiActionFactory(source);
        TimeoutTasks = source.AddAction
        (
            actions.Action(nameof(TimeoutTasks), () => sp.GetRequiredService<TimeoutTasksAction>())
        );
        PurgeJobsAndEvents = source.AddAction
        (
            actions.Action(nameof(PurgeJobsAndEvents), () => sp.GetRequiredService<PurgeJobsAndEventsAction>())
        );
    }

    public AppApiAction<EmptyRequest, EmptyActionResult> TimeoutTasks { get; }
    public AppApiAction<EmptyRequest, EmptyActionResult> PurgeJobsAndEvents { get; }
}