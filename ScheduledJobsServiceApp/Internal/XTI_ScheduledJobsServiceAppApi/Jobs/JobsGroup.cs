using XTI_Core;
using XTI_Schedule;

namespace XTI_ScheduledJobsServiceAppApi.Jobs;

public sealed class JobsGroup : AppApiGroupWrapper
{
    public JobsGroup(AppApiGroup source, IServiceProvider sp)
        : base(source)
    {
        AddJobScheduleNotifications = source.AddAction<EmptyRequest, EmptyActionResult>()
            .Named(nameof(AddJobScheduleNotifications))
            .WithExecution<AddJobScheduleNotificationsAction>()
            .RunContinuously()
                .Interval(TimeSpan.FromHours(4))
                .AddSchedule
                (
                    Schedule.EveryDay().At(TimeRange.AllDay())
                )
            .Build();
        PurgeJobsAndEvents = source.AddAction<EmptyRequest, EmptyActionResult>()
            .Named(nameof(PurgeJobsAndEvents))
            .WithExecution<PurgeJobsAndEventsAction>()
            .RunUntilSuccess()
                .Interval(TimeSpan.FromMinutes(15))
                .AddSchedule
                (
                    Schedule.EveryDay().At(TimeRange.From(1).ForOneHour())
                )
            .Build();
        TimeoutJobs = source.AddAction<EmptyRequest, EmptyActionResult>()
            .Named(nameof(TimeoutJobs))
            .WithExecution<TimeoutJobsAction>()
            .ThrottleRequestLogging().ForOneHour()
            .RunContinuously()
                .Interval(TimeSpan.FromMinutes(15))
                .AddSchedule
                (
                    Schedule.EveryDay().At(TimeRange.AllDay())
                )
            .Build();
    }

    public AppApiAction<EmptyRequest, EmptyActionResult> AddJobScheduleNotifications { get; }
    public AppApiAction<EmptyRequest, EmptyActionResult> PurgeJobsAndEvents { get; }
    public AppApiAction<EmptyRequest, EmptyActionResult> TimeoutJobs { get; }
}