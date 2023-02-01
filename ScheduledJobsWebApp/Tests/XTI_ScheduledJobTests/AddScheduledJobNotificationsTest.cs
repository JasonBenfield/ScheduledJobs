using System;
using XTI_Core;
using XTI_Core.Fakes;
using XTI_Schedule;
using XTI_ScheduledJobsWebAppApi;

namespace XTI_ScheduledJobTests;

internal sealed class AddScheduledJobNotificationsTest
{
    [Test]
    public async Task ShouldAddScheduledJobNotifications()
    {
        var host = TestHost.CreateDefault();
        var clock = host.GetRequiredService<FakeClock>();
        clock.Set(new DateTime(2023, 1, 31, 8, 0, 0));
        await host.Register
        (
            events => { },
            jobs => BuildJobs(jobs)
        );
        await host.RegisterJobSchedule
        (
            DemoJobs.DoSomething.JobKey,
            Schedule.Second(DayOfWeek.Wednesday).OfEveryMonth().At(TimeRange.From(10).ForOneHour())
        );
        clock.Set(new DateTime(2023, 2, 8, 8, 0, 0));
        var api = host.GetRequiredService<ScheduledJobsAppApi>();
        await api.Events.AddJobScheduleNotifications.Invoke(new EmptyRequest());
        clock.Set(new DateTime(2023, 2, 8, 10, 15, 0));
        var triggeredJobs = await host.MonitorScheduledJob(DemoJobs.DoSomething.JobKey);
        Assert.That(triggeredJobs.Length, Is.EqualTo(1), "Should add scheduled job notifications");
        clock.Set(new DateTime(2023, 2, 9, 8, 0, 0));
        await api.Events.AddJobScheduleNotifications.Invoke(new EmptyRequest());
        clock.Set(new DateTime(2023, 2, 9, 10, 15, 0));
        triggeredJobs = await host.MonitorScheduledJob(DemoJobs.DoSomething.JobKey);
        Assert.That(triggeredJobs.Length, Is.EqualTo(0), "Should not trigger job when not scheduled");
    }

    private static JobRegistration BuildJobs(JobRegistration jobs) =>
        jobs.AddJob
        (
            DemoJobs.DoSomething.JobKey,
            j =>
            {
                foreach (var task in DemoJobs.DoSomething.GetAllTasks())
                {
                    j.AddTask(task);
                }
            }
        )
        .AddJob
        (
            DemoJobs.DoSomethingElse.JobKey,
            j =>
            {
                foreach (var task in DemoJobs.DoSomethingElse.GetAllTasks())
                {
                    j.AddTask(task);
                }
            }
        );
}
