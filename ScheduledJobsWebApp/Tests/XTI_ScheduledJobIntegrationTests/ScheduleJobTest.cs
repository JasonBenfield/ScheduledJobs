using XTI_Core;
using XTI_Schedule;

namespace XTI_ScheduledJobIntegrationTests;

internal sealed class ScheduleJobTest
{
    [Test]
    public async Task ShouldScheduleJob()
    {
        var host = TestHost.CreateDefault(XtiEnvironment.Development);
        await host.Setup();
        await host.RegisterJobs
        (
            jobs => BuildJobs(jobs)
        );
        await host.RegisterJobSchedule
        (
            DemoJobs.DoSomething.JobKey,
            Schedule.EveryDay().At(TimeRange.From(new TimeOnly(11, 30)).ForOneHour()),
            Schedule.On(DayOfWeek.Thursday).At
            (
                TimeRange.From(new TimeOnly(8, 0)).ForOneHour(),
                TimeRange.From(new TimeOnly(14, 0)).ForOneHour()
            )
        );
        await host.MonitorScheduledJob(DemoJobs.DoSomething.JobKey);
    }

    private static JobRegistrationBuilder1 BuildJobs(JobRegistrationBuilder jobs) =>
        jobs
            .AddJob(DemoJobs.DoSomething.JobKey)
            .TimeoutAfter(TimeSpan.FromHours(1))
            .AddTasks
            (
                DemoJobs.DoSomething.GetAllTasks(),
                (t, j) => j.TimeoutAfter(TimeSpan.FromMinutes(5))
            );
}
