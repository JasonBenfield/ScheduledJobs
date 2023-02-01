using System;
using XTI_Core;
using XTI_Core.Fakes;
using XTI_Schedule;

namespace XTI_ScheduledJobTests;

internal sealed class ScheduleJobTest
{
    [Test]
    public async Task ShouldTriggerJob_WhenScheduled()
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
            Schedule.EveryDay().At(TimeRange.From(10).ForOneHour())
        );
        clock.Set(new DateTime(2023, 1, 31, 10, 15, 0));
        var triggeredJobs = await host.MonitorScheduledJob(DemoJobs.DoSomething.JobKey);
        Assert.That(triggeredJobs.Length, Is.EqualTo(1), "Should trigger job when scheduled");
    }

    [Test]
    public async Task ShouldCompleteJob_WhenScheduled()
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
            Schedule.EveryDay().At(TimeRange.From(10).ForOneHour())
        );
        clock.Set(new DateTime(2023, 1, 31, 10, 15, 0));
        var triggeredJobs = await host.MonitorScheduledJob(DemoJobs.DoSomething.JobKey);
        Assert.That(triggeredJobs[0].Status(), Is.EqualTo(JobTaskStatus.Values.Completed), "Should complete job when scheduled");
    }

    [Test]
    public async Task ShouldNotTriggerJobMultipleTimes_ForTheSameSchedule()
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
            Schedule.EveryDay().At(TimeRange.From(10).ForOneHour())
        );
        clock.Set(new DateTime(2023, 1, 31, 10, 15, 0));
        await host.MonitorScheduledJob(DemoJobs.DoSomething.JobKey);
        clock.Set(new DateTime(2023, 1, 31, 10, 30, 0));
        var triggeredJobs = await host.MonitorScheduledJob(DemoJobs.DoSomething.JobKey);
        Assert.That(triggeredJobs.Length, Is.EqualTo(0), "Should not trigger job multiple times for the same schedule");
    }

    [Test]
    public async Task ShouldNotTriggerJob_WhenNotScheduled()
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
            Schedule.EveryDay().At(TimeRange.From(10).ForOneHour())
        );
        clock.Set(new DateTime(2023, 1, 31, 11, 15, 0));
        var triggeredJobs = await host.MonitorScheduledJob(DemoJobs.DoSomething.JobKey);
        Assert.That(triggeredJobs.Length, Is.EqualTo(0), "Should not trigger job when not scheduled");
    }

    [Test]
    public async Task ShouldNotTriggerJob_WhenNotScheduledAfterScheduleChanged()
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
            Schedule.EveryDay().At(TimeRange.From(10).ForOneHour())
        );
        await host.RegisterJobSchedule
        (
            DemoJobs.DoSomething.JobKey,
            Schedule.EveryDay().At(TimeRange.From(11).ForOneHour())
        );
        clock.Set(new DateTime(2023, 1, 31, 10, 15, 0));
        var triggeredJobs1 = await host.MonitorScheduledJob(DemoJobs.DoSomething.JobKey);
        Assert.That(triggeredJobs1.Length, Is.EqualTo(0), "Should not trigger job when not scheduled after schedule changed");
        clock.Set(new DateTime(2023, 1, 31, 11, 15, 0));
        var triggeredJobs2 = await host.MonitorScheduledJob(DemoJobs.DoSomething.JobKey);
        Assert.That(triggeredJobs2.Length, Is.EqualTo(1), "Should trigger job when scheduled after schedule change");
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
