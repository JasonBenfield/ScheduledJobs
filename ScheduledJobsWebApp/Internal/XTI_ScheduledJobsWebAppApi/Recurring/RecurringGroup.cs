namespace XTI_ScheduledJobsWebAppApi.Recurring;

public sealed class RecurringGroup : AppApiGroupWrapper
{
    public RecurringGroup(AppApiGroup source, IServiceProvider sp)
        : base(source)
    {
        AddJobScheduleNotifications = source.AddAction<EmptyRequest, EmptyActionResult>()
            .Named(nameof(AddJobScheduleNotifications))
            .WithExecution<AddJobScheduleNotificationsAction>()
            .ThrottleRequestLogging().ForOneHour()
            .ThrottleExceptionLogging().For(5).Minutes()
            .Build();
        TimeoutTasks = source.AddAction<EmptyRequest, EmptyActionResult>()
            .Named(nameof(TimeoutTasks))
            .WithExecution<TimeoutTasksAction>()
            .ThrottleRequestLogging().ForOneHour()
            .ThrottleExceptionLogging().For(5).Minutes()
            .Build();
        PurgeJobsAndEvents = source.AddAction<EmptyRequest, EmptyActionResult>()
            .Named(nameof(PurgeJobsAndEvents))
            .WithExecution<PurgeJobsAndEventsAction>()
            .Build();
    }

    public AppApiAction<EmptyRequest, EmptyActionResult> AddJobScheduleNotifications { get; }
    public AppApiAction<EmptyRequest, EmptyActionResult> TimeoutTasks { get; }
    public AppApiAction<EmptyRequest, EmptyActionResult> PurgeJobsAndEvents { get; }
}