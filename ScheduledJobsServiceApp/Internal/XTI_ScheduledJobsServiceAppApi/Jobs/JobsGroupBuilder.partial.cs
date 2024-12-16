using XTI_Core;
using XTI_Schedule;

namespace XTI_ScheduledJobsServiceAppApi.Jobs;

partial class JobsGroupBuilder
{
    partial void Configure()
    {
        AddJobScheduleNotifications
            .RunContinuously()
                .Interval(TimeSpan.FromHours(4))
                .AddSchedule
                (
                    Schedule.EveryDay().At(TimeRange.AllDay())
                );
        PurgeJobsAndEvents
            .RunUntilSuccess()
                .Interval(TimeSpan.FromMinutes(15))
                .AddSchedule
                (
                    Schedule.EveryDay().At(TimeRange.From(1).ForOneHour())
                );
        TimeoutJobs
            .ThrottleRequestLogging().ForOneHour()
            .RunContinuously()
                .Interval(TimeSpan.FromMinutes(15))
                .AddSchedule
                (
                    Schedule.EveryDay().At(TimeRange.AllDay())
                );
    }
}
