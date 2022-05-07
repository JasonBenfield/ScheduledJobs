using System;
using XTI_Core.Extensions;

namespace XTI_ScheduledJobTests;

internal sealed class EventNotificationTest
{
    [Test]
    public async Task ShouldTriggerJob_WhenEventOccurs()
    {
        var host = TestHost.CreateDefault();
        await Register
        (
            host,
            events => events.AddEvent(DemoEventKeys.SomethingHappened),
            jobs => jobs.AddJob(DemoJobKeys.DoSomething, _ => { })
        );
        var eventNotifications = await RaiseEvent
        (
            host,
            DemoEventKeys.SomethingHappened,
            new EventSource("1", "{ \"ID\": 1 }")
        );
        await MonitorEvent(host, DemoEventKeys.SomethingHappened, DemoJobKeys.DoSomething);
        var triggeredJobs = await eventNotifications[0].TriggeredJobs();
        Assert.That
        (
            triggeredJobs.Length,
            Is.EqualTo(1),
            "Should trigger job when event is raised that job is subscribed to."
        );
    }

    [Test]
    public async Task ShouldNotTriggerJob_WhenNotSubscribedToTheEvent()
    {
        var host = TestHost.CreateDefault();
        await Register
        (
            host,
            events => events
                .AddEvent(DemoEventKeys.SomethingHappened)
                .AddEvent(DemoEventKeys.SomethingElseHappened),
            jobs => jobs.AddJob(DemoJobKeys.DoSomething, _ => { })
        );
        var eventNotifications = await RaiseEvent
        (
            host,
            DemoEventKeys.SomethingElseHappened,
            new EventSource("1", "{ \"ID\": 1 }")
        );
        await MonitorEvent(host, DemoEventKeys.SomethingHappened, DemoJobKeys.DoSomething);
        var triggeredJobs = await eventNotifications[0].TriggeredJobs();
        Assert.That
        (
            triggeredJobs.Length,
            Is.EqualTo(0),
            "Should not trigger job when event is raised that job is not subscribed to."
        );
    }

    [Test]
    public async Task ShouldTriggerJobOnceForEachEvent()
    {
        var host = TestHost.CreateDefault();
        await Register
        (
            host,
            events => events.AddEvent(DemoEventKeys.SomethingHappened),
            jobs => jobs.AddJob(DemoJobKeys.DoSomething, _ => { })
        );
        var eventNotifications = await RaiseEvent
        (
            host,
            DemoEventKeys.SomethingHappened,
            new EventSource("1", "{ \"ID\": 1 }")
        );
        await MonitorEvent(host, DemoEventKeys.SomethingHappened, DemoJobKeys.DoSomething);
        await MonitorEvent(host, DemoEventKeys.SomethingHappened, DemoJobKeys.DoSomething);
        var triggeredJobs = await eventNotifications[0].TriggeredJobs();
        Assert.That
        (
            triggeredJobs.Length,
            Is.EqualTo(1),
            "Should trigger job once for each event."
        );
    }

    [Test]
    public async Task ShouldIgnoreDuplicateEvents()
    {
        var host = TestHost.CreateDefault();
        await Register
        (
            host,
            events => events.AddEvent
            (
                DemoEventKeys.SomethingHappened,
                evt => evt.Ignore().WhenSourceKeysAndDataAreEqual()
            ),
            jobs => jobs.AddJob(DemoJobKeys.DoSomething, _ => { })
        );
        var eventNotifications1 = await RaiseEvent
        (
            host,
            DemoEventKeys.SomethingHappened,
            new EventSource("1", "{ \"ID\": 1 }")
        );
        var eventNotifications2 = await RaiseEvent
        (
            host,
            DemoEventKeys.SomethingHappened,
            new EventSource("1", "{ \"ID\": 1 }")
        );
        Assert.That(eventNotifications2.Length, Is.EqualTo(0), "Should ignore duplicate events.");
    }

    [Test]
    public async Task ShouldKeepAllDuplicateEvents()
    {
        var host = TestHost.CreateDefault();
        await Register
        (
            host,
            events => events.AddEvent
            (
                DemoEventKeys.SomethingHappened,
                evt => evt.KeepAll().WhenSourceKeysAndDataAreEqual()
            ),
            jobs => jobs.AddJob(DemoJobKeys.DoSomething, _ => { })
        );
        var eventNotifications1 = await RaiseEvent
        (
            host,
            DemoEventKeys.SomethingHappened,
            new EventSource("1", "{ \"ID\": 1 }")
        );
        var eventNotifications2 = await RaiseEvent
        (
            host,
            DemoEventKeys.SomethingHappened,
            new EventSource("1", "{ \"ID\": 1 }")
        );
        await MonitorEvent(host, DemoEventKeys.SomethingHappened, DemoJobKeys.DoSomething);
        var triggeredJobs1 = await eventNotifications1[0].TriggeredJobs();
        Assert.That
        (
            triggeredJobs1.Length,
            Is.EqualTo(1),
            "Should trigger job for first event."
        );
        var triggeredJobs2 = await eventNotifications2[0].TriggeredJobs();
        Assert.That
        (
            triggeredJobs2.Length,
            Is.EqualTo(1),
            "Should trigger job for duplicate event."
        );
    }

    [Test]
    public async Task ShouldKeepOnlyOldestEvent_WhenNewDuplicatesArrive()
    {
        var host = TestHost.CreateDefault();
        await Register
        (
            host,
            events => events.AddEvent
            (
                DemoEventKeys.SomethingHappened,
                evt => evt.KeepOldest().WhenSourceKeysAndDataAreEqual()
            ),
            jobs => jobs.AddJob(DemoJobKeys.DoSomething, _ => { })
        );
        var eventNotifications1 = await RaiseEvent
        (
            host,
            DemoEventKeys.SomethingHappened,
            new EventSource("1", "{ \"ID\": 1 }")
        );
        var eventNotifications2 = await RaiseEvent
        (
            host,
            DemoEventKeys.SomethingHappened,
            new EventSource("1", "{ \"ID\": 1 }")
        );
        await MonitorEvent(host, DemoEventKeys.SomethingHappened, DemoJobKeys.DoSomething);
        var triggeredJobs1 = await eventNotifications1[0].TriggeredJobs();
        Assert.That
        (
            triggeredJobs1.Length,
            Is.EqualTo(1),
            "Should trigger job for first event."
        );
        var triggeredJobs2 = await eventNotifications2[0].TriggeredJobs();
        Assert.That
        (
            triggeredJobs2.Length,
            Is.EqualTo(0),
            "Should not trigger job for newer duplicate event."
        );
    }

    [Test]
    public async Task ShouldKeepOnlyNewestEvent_WhenNewDuplicatesArrive()
    {
        var host = TestHost.CreateDefault();
        await Register
        (
            host,
            events => events.AddEvent
            (
                DemoEventKeys.SomethingHappened,
                evt => evt.KeepNewest().WhenSourceKeysAndDataAreEqual()
            ),
            jobs => jobs.AddJob(DemoJobKeys.DoSomething, _ => { })
        );
        var eventNotifications1 = await RaiseEvent
        (
            host,
            DemoEventKeys.SomethingHappened,
            new EventSource("1", "{ \"ID\": 1 }")
        );
        var eventNotifications2 = await RaiseEvent
        (
            host,
            DemoEventKeys.SomethingHappened,
            new EventSource("1", "{ \"ID\": 1 }")
        );
        await MonitorEvent(host, DemoEventKeys.SomethingHappened, DemoJobKeys.DoSomething);
        var triggeredJobs1 = await eventNotifications1[0].TriggeredJobs();
        Assert.That
        (
            triggeredJobs1.Length,
            Is.EqualTo(0),
            "Should not trigger job for older events."
        );
        var triggeredJobs2 = await eventNotifications2[0].TriggeredJobs();
        Assert.That
        (
            triggeredJobs2.Length,
            Is.EqualTo(1),
            "Should trigger job for newest duplicate event."
        );
    }

    [Test]
    public async Task ShouldDetermineDuplicateBaseOnSourceKeyOnly()
    {
        var host = TestHost.CreateDefault();
        await Register
        (
            host,
            events => events.AddEvent
            (
                DemoEventKeys.SomethingHappened,
                evt => evt.Ignore().WhenSourceKeysOnlyAreEqual()
            ),
            jobs => jobs.AddJob(DemoJobKeys.DoSomething, _ => { })
        );
        var eventNotifications1 = await RaiseEvent
        (
            host,
            DemoEventKeys.SomethingHappened,
            new EventSource("1", "{ \"ID\": 1, \"Description\": \"Whatever\" }")
        );
        var eventNotifications2 = await RaiseEvent
        (
            host,
            DemoEventKeys.SomethingHappened,
            new EventSource("1", "{ \"ID\": 1, \"Description\": \"Different\" }")
        );
        Assert.That(eventNotifications2.Length, Is.EqualTo(0), "Should handle events as duplicates when source keys only are equal");
    }

    private async Task Register(XtiHost host, Action<EventRegistration> configEvents, Action<JobRegistration> configJobs)
    {
        var events = host.GetRequiredService<EventRegistration>();
        configEvents(events);
        await events.Register();
        var jobs = host.GetRequiredService<JobRegistration>();
        configJobs(jobs);
        await jobs.Register();
    }

    private static Task MonitorEvent(XtiHost host, EventKey eventKey, JobKey jobKey)
    {
        var monitorFactory = host.GetRequiredService<EventMonitorFactory>();
        var monitor = monitorFactory
            .When(eventKey)
            .Trigger(jobKey);
        return monitor.Run();
    }

    private static Task<EventNotification[]> RaiseEvent(XtiHost host, EventKey eventKey, EventSource source)
    {
        var incomingEventFactory = host.GetRequiredService<IncomingEventFactory>();
        return incomingEventFactory
            .Incoming(eventKey)
            .From(source)
            .Notify();
    }

}
