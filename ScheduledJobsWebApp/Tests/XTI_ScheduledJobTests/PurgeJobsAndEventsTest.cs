using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using XTI_App.Api;
using XTI_Core;
using XTI_JobsDB.EF;
using XTI_ScheduledJobsWebAppApi;

namespace XTI_ScheduledJobTests;

internal sealed class PurgeJobsAndEventsTest
{
    [Test]
    public async Task ShouldDeleteLogEntries_AfterTimeToDelete()
    {
        var host = TestHost.CreateDefault();
        await host.Register
        (
            events => events.AddEvent(DemoEventKeys.SomethingHappened),
            jobs => BuildJobs(jobs)
        );
        var somethingHappenedData = new SomethingHappenedData
        {
            ID = 2,
            Items = Enumerable.Range(1, 3).ToArray()
        };
        var eventNotifications = await host.RaiseEvent
        (
            DemoEventKeys.SomethingHappened,
            new EventSource
            (
                somethingHappenedData.ID.ToString(),
                XtiSerializer.Serialize(somethingHappenedData)
            )
        );
        var demoContext01 = host.GetRequiredService<DemoItemActionContext<DemoItemAction01>>();
        demoContext01.ThrowErrorWhen("Whatever", data => data.ItemID == 2);
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        host.FastForward(TimeSpan.FromDays(366));
        var api = host.GetRequiredService<ScheduledJobsAppApi>();
        await api.Recurring.PurgeJobsAndEvents.Invoke(new EmptyRequest());
        var db = host.GetRequiredService<JobDbContext>();
        var logEntries = await db.LogEntries.Retrieve().ToArrayAsync();
        Assert.That(logEntries.Length, Is.EqualTo(0), "Should delete log entries");
    }

    [Test]
    public async Task ShouldDeleteHierarchicalTasks_AfterTimeToDelete()
    {
        var host = TestHost.CreateDefault();
        await host.Register
        (
            events => events.AddEvent(DemoEventKeys.SomethingHappened),
            jobs => BuildJobs(jobs)
        );
        var somethingHappenedData = new SomethingHappenedData { ID = 2, Items = Enumerable.Range(1, 3).ToArray() };
        var eventNotifications = await host.RaiseEvent
        (
            DemoEventKeys.SomethingHappened,
            new EventSource
            (
                somethingHappenedData.ID.ToString(),
                XtiSerializer.Serialize(somethingHappenedData)
            )
        );
        var demoContext01 = host.GetRequiredService<DemoItemActionContext<DemoItemAction01>>();
        demoContext01.ThrowErrorWhen("Whatever", data => data.ItemID == 2);
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        host.FastForward(TimeSpan.FromDays(366));
        var api = host.GetRequiredService<ScheduledJobsAppApi>();
        await api.Recurring.PurgeJobsAndEvents.Invoke(new EmptyRequest());
        var db = host.GetRequiredService<JobDbContext>();
        var hierarchicalTasks = await db.HierarchicalTriggeredJobTasks.Retrieve().ToArrayAsync();
        Assert.That(hierarchicalTasks.Length, Is.EqualTo(0), "Should delete hierarchical triggered job tasks");
    }

    [Test]
    public async Task ShouldDeleteTasks_AfterTimeToDelete()
    {
        var host = TestHost.CreateDefault();
        await host.Register
        (
            events => events.AddEvent(DemoEventKeys.SomethingHappened),
            jobs => BuildJobs(jobs)
        );
        var somethingHappenedData = new SomethingHappenedData { ID = 2, Items = Enumerable.Range(1, 3).ToArray() };
        var eventNotifications = await host.RaiseEvent
        (
            DemoEventKeys.SomethingHappened,
            new EventSource
            (
                somethingHappenedData.ID.ToString(),
                XtiSerializer.Serialize(somethingHappenedData)
            )
        );
        var demoContext01 = host.GetRequiredService<DemoItemActionContext<DemoItemAction01>>();
        demoContext01.ThrowErrorWhen("Whatever", data => data.ItemID == 2);
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        host.FastForward(TimeSpan.FromDays(366));
        var api = host.GetRequiredService<ScheduledJobsAppApi>();
        await api.Recurring.PurgeJobsAndEvents.Invoke(new EmptyRequest());
        var db = host.GetRequiredService<JobDbContext>();
        var tasks = await db.TriggeredJobTasks.Retrieve().ToArrayAsync();
        Assert.That(tasks.Length, Is.EqualTo(0), "Should delete triggered job tasks");
    }

    [Test]
    public async Task ShouldDeleteJobs_AfterTimeToDelete()
    {
        var host = TestHost.CreateDefault();
        await host.Register
        (
            events => events.AddEvent(DemoEventKeys.SomethingHappened),
            jobs => BuildJobs(jobs)
        );
        var somethingHappenedData = new SomethingHappenedData { ID = 2, Items = Enumerable.Range(1, 3).ToArray() };
        var eventNotifications = await host.RaiseEvent
        (
            DemoEventKeys.SomethingHappened,
            new EventSource
            (
                somethingHappenedData.ID.ToString(),
                XtiSerializer.Serialize(somethingHappenedData)
            )
        );
        var demoContext01 = host.GetRequiredService<DemoItemActionContext<DemoItemAction01>>();
        demoContext01.ThrowErrorWhen("Whatever", data => data.ItemID == 2);
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        host.FastForward(TimeSpan.FromDays(366));
        var api = host.GetRequiredService<ScheduledJobsAppApi>();
        await api.Recurring.PurgeJobsAndEvents.Invoke(new EmptyRequest());
        var db = host.GetRequiredService<JobDbContext>();
        var jobs = await db.TriggeredJobs.Retrieve().ToArrayAsync();
        Assert.That(jobs.Length, Is.EqualTo(0), "Should delete triggered jobs");
    }

    [Test]
    public async Task ShouldDeleteEventNotifications_AfterTimeToDelete()
    {
        var host = TestHost.CreateDefault();
        await host.Register
        (
            events => events.AddEvent
            (
                DemoEventKeys.SomethingHappened,
                evt => evt.DeleteAfter(TimeSpan.FromDays(365))
            ),
            jobs => BuildJobs(jobs)
        );
        var somethingHappenedData = new SomethingHappenedData { ID = 2, Items = Enumerable.Range(1, 3).ToArray() };
        await host.RaiseEvent
        (
            DemoEventKeys.SomethingHappened,
            new EventSource
            (
                somethingHappenedData.ID.ToString(),
                XtiSerializer.Serialize(somethingHappenedData)
            )
        );
        host.FastForward(TimeSpan.FromDays(366));
        var api = host.GetRequiredService<ScheduledJobsAppApi>();
        await api.Recurring.PurgeJobsAndEvents.Invoke(new EmptyRequest());
        var db = host.GetRequiredService<JobDbContext>();
        var eventNotifications = await db.EventNotifications.Retrieve().ToArrayAsync();
        Assert.That(eventNotifications.Length, Is.EqualTo(0), "Should delete event notifications");
    }

    [Test]
    public async Task ShouldNotDeleteEventNotifications_WhenTriggeredJobHasNotBeenDeleted()
    {
        var host = TestHost.CreateDefault();
        await host.Register
        (
            events => events.AddEvent
            (
                DemoEventKeys.SomethingHappened,
                evt => evt.DeleteAfter(TimeSpan.FromDays(1))
            ),
            jobs => BuildJobs(jobs)
        );
        var somethingHappenedData = new SomethingHappenedData { ID = 2, Items = Enumerable.Range(1, 3).ToArray() };
        await host.RaiseEvent
        (
            DemoEventKeys.SomethingHappened,
            new EventSource
            (
                somethingHappenedData.ID.ToString(),
                XtiSerializer.Serialize(somethingHappenedData)
            )
        );
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        host.FastForward(TimeSpan.FromDays(2));
        var api = host.GetRequiredService<ScheduledJobsAppApi>();
        await api.Recurring.PurgeJobsAndEvents.Invoke(new EmptyRequest());
        var db = host.GetRequiredService<JobDbContext>();
        var eventNotifications = await db.EventNotifications.Retrieve().ToArrayAsync();
        Assert.That(eventNotifications.Length, Is.EqualTo(1), "Should not delete event notification when triggered job has not been deleted");
    }

    private static JobRegistration BuildJobs(JobRegistration jobs) =>
        jobs.AddJob
        (
            DemoJobs.DoSomething.JobKey,
            job => job
                .TimeoutAfter(TimeSpan.FromHours(1))
                .DeleteAfter(TimeSpan.FromDays(365))
                .AddTask(DemoJobs.DoSomething.Task01).TimeoutAfter(TimeSpan.FromMinutes(5))
                .AddTask(DemoJobs.DoSomething.Task02).TimeoutAfter(TimeSpan.FromMinutes(5))
                .AddTask(DemoJobs.DoSomething.TaskItem01).TimeoutAfter(TimeSpan.FromMinutes(5))
                .AddTask(DemoJobs.DoSomething.TaskItem02).TimeoutAfter(TimeSpan.FromMinutes(5))
                .AddTask(DemoJobs.DoSomething.TaskFinal).TimeoutAfter(TimeSpan.FromMinutes(5))
        );
}
