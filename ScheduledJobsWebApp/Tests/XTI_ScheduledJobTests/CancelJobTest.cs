using Microsoft.EntityFrameworkCore;
using System;
using XTI_App.Api;
using XTI_Core.Fakes;
using XTI_JobsDB.EF;
using XTI_ScheduledJobsWebAppApi;

namespace XTI_ScheduledJobTests;

internal sealed class CancelJobTest
{
    [Test]
    public async Task ShouldCancelJob()
    {
        var host = TestHost.CreateDefault();
        await host.Register
        (
            events => events.AddEvent(DemoEventKeys.SomethingHappened),
            jobs => jobs.BuildJobs()
        );
        var sourceData = new SomethingHappenedData
        {
            ID = 2,
            Items = Enumerable.Range(1, 3).ToArray()
        };
        var eventNotifications = await host.RaiseEvent
        (
            DemoEventKeys.SomethingHappened,
            new EventSource(sourceData.ID.ToString(), JsonSerializer.Serialize(sourceData))
        );
        var demoContext = host.GetRequiredService<DemoItemActionContext<DemoItemAction01>>();
        demoContext.CancelWhen
        (
            data => data.ItemID == 2,
            throwError => throwError.Because("Stone Cold said so").Throw()
        );
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        var triggeredJobs = await eventNotifications[0].TriggeredJobs();
        Assert.That
        (
            triggeredJobs[0].Status(),
            Is.EqualTo(JobTaskStatus.Values.Canceled),
            "Should cancel job"
        );
    }

    [Test]
    public async Task ShouldDeleteJobTheNextDay()
    {
        var host = TestHost.CreateDefault();
        await host.Register
        (
            events => events.AddEvent(DemoEventKeys.SomethingHappened),
            jobs => jobs.BuildJobs()
        );
        var sourceData = new SomethingHappenedData
        {
            ID = 2,
            Items = Enumerable.Range(1, 3).ToArray()
        };
        var clock = host.GetRequiredService<FakeClock>();
        clock.Set(new DateTimeOffset(new DateTime(2022, 06, 09, 07, 45, 00)));
        var eventNotifications = await host.RaiseEvent
        (
            DemoEventKeys.SomethingHappened,
            new EventSource(sourceData.ID.ToString(), JsonSerializer.Serialize(sourceData))
        );
        var demoContext = host.GetRequiredService<DemoItemActionContext<DemoItemAction01>>();
        demoContext.CancelWhen
        (
            data => data.ItemID == 2,
            throwError => throwError.Because("Stone Cold said so").Throw()
        );
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        var api = host.GetRequiredService<ScheduledJobsAppApi>();
        clock.Set(new DateTimeOffset(new DateTime(2022, 06, 09, 23, 59, 00)));
        await api.Recurring.PurgeJobsAndEvents.Invoke(new EmptyRequest());
        var db = host.GetRequiredService<JobDbContext>();
        var jobs1 = await db.TriggeredJobs.Retrieve().ToArrayAsync();
        Assert.That(jobs1.Length, Is.EqualTo(1), "Should not delete job before the next day");
        clock.Set(new DateTimeOffset(new DateTime(2022, 06, 10, 1, 05, 00)));
        await api.Recurring.PurgeJobsAndEvents.Invoke(new EmptyRequest());
        var jobs2 = await db.TriggeredJobs.Retrieve().ToArrayAsync();
        Assert.That(jobs2.Length, Is.EqualTo(0), "Should delete job after the next day");
    }

    [Test]
    public async Task ShouldDeleteJobAtTheNormalTime()
    {
        var host = TestHost.CreateDefault();
        await host.Register
        (
            events => events.AddEvent(DemoEventKeys.SomethingHappened),
            jobs => jobs.BuildJobs()
        );
        var sourceData = new SomethingHappenedData
        {
            ID = 2,
            Items = Enumerable.Range(1, 3).ToArray()
        };
        var eventNotifications = await host.RaiseEvent
        (
            DemoEventKeys.SomethingHappened,
            new EventSource(sourceData.ID.ToString(), JsonSerializer.Serialize(sourceData))
        );
        var demoContext = host.GetRequiredService<DemoItemActionContext<DemoItemAction01>>();
        demoContext.CancelWhen
        (
            data => data.ItemID == 2,
            throwError => throwError.Because("Stone Cold said so").DeleteAtNormalTime().Throw()
        );
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        host.FastForward(TimeSpan.FromDays(364));
        var api = host.GetRequiredService<ScheduledJobsAppApi>();
        await api.Recurring.PurgeJobsAndEvents.Invoke(new EmptyRequest());
        var db = host.GetRequiredService<JobDbContext>();
        var jobs1 = await db.TriggeredJobs.Retrieve().ToArrayAsync();
        Assert.That(jobs1.Length, Is.EqualTo(1), "Should not delete job before the normal time");
        host.FastForward(TimeSpan.FromDays(2));
        await api.Recurring.PurgeJobsAndEvents.Invoke(new EmptyRequest());
        var jobs2 = await db.TriggeredJobs.Retrieve().ToArrayAsync();
        Assert.That(jobs2.Length, Is.EqualTo(0), "Should delete job after the normal time");
    }

    [Test]
    public async Task ShouldLogReason()
    {
        var host = TestHost.CreateDefault();
        await host.Register
        (
            events => events.AddEvent(DemoEventKeys.SomethingHappened),
            jobs => jobs.BuildJobs()
        );
        var sourceData = new SomethingHappenedData
        {
            ID = 2,
            Items = Enumerable.Range(1, 3).ToArray()
        };
        var eventNotifications = await host.RaiseEvent
        (
            DemoEventKeys.SomethingHappened,
            new EventSource(sourceData.ID.ToString(), JsonSerializer.Serialize(sourceData))
        );
        var demoContext = host.GetRequiredService<DemoItemActionContext<DemoItemAction01>>();
        const string reason = "Stone Cold said so";
        demoContext.CancelWhen
        (
            data => data.ItemID == 2,
            throwError => throwError.Because(reason).Throw()
        );
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        var triggeredJobs = await eventNotifications[0].TriggeredJobs();
        var messages = triggeredJobs[0].Messages();
        Assert.That
        (
            messages.Select(m => m.Category),
            Is.EqualTo(new[] { "Cancelled" }),
            "Should log cancellation reason"
        );
        Assert.That
        (
            messages.Select(m => m.Message),
            Is.EqualTo(new[] { reason }),
            "Should log cancellation reason"
        );
    }

    [Test]
    public async Task ShouldDeleteImmediately()
    {
        var host = TestHost.CreateDefault();
        await host.Register
        (
            events => events.AddEvent(DemoEventKeys.SomethingHappened),
            jobs => jobs.BuildJobs()
        );
        var sourceData = new SomethingHappenedData
        {
            ID = 2,
            Items = Enumerable.Range(1, 3).ToArray()
        };
        var eventNotifications = await host.RaiseEvent
        (
            DemoEventKeys.SomethingHappened,
            new EventSource(sourceData.ID.ToString(), JsonSerializer.Serialize(sourceData))
        );
        var demoContext = host.GetRequiredService<DemoItemActionContext<DemoItemAction01>>();
        demoContext.CancelWhen
        (
            data => data.ItemID == 2,
            throwError => throwError.DeleteImmediately().Throw()
        );
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        var db = host.GetRequiredService<JobDbContext>();
        var jobs = await db.TriggeredJobs.Retrieve().ToArrayAsync();
        Assert.That(jobs.Length, Is.EqualTo(0), "Should delete job immediately");
    }
}
