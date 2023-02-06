using System;
using XTI_Core;
using XTI_Core.Fakes;

namespace XTI_ScheduledJobTests;

internal sealed class TriggerJobTest
{
    [Test]
    public async Task ShouldTriggerJob_WhenEventOccurs()
    {
        var host = TestHost.CreateDefault();
        await host.Register
        (
            events => events.AddEvent(DemoEventKeys.SomethingHappened),
            jobs => BuildJobs(jobs)
        );
        var eventNotifications = await host.RaiseEvent
        (
            DemoEventKeys.SomethingHappened,
            new XtiEventSource("1", "{ \"ID\": 1 }")
        );
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        var triggeredJobs = await eventNotifications[0].TriggeredJobs();
        Assert.That
        (
            triggeredJobs.Length,
            Is.EqualTo(1),
            "Should trigger job when event is raised that job is subscribed to."
        );
    }

    [Test]
    public async Task ShouldTriggerJobWithTheSameTaskName()
    {
        var host = TestHost.CreateDefault();
        await host.Register
        (
            events => events.AddEvent(DemoEventKeys.SomethingElseHappened),
            jobs => BuildJobs(jobs)
        );
        var eventNotifications = await host.RaiseEvent
        (
            DemoEventKeys.SomethingElseHappened,
            new XtiEventSource("1", "{ \"ID\": 1 }")
        );
        await host.MonitorEvent(DemoEventKeys.SomethingElseHappened, DemoJobs.DoSomethingElse.JobKey);
        var triggeredJobs = await eventNotifications[0].TriggeredJobs();
        var errors = triggeredJobs[0].Errors();
        Console.WriteLine(XtiSerializer.Serialize(errors, new JsonSerializerOptions { WriteIndented = true }));
        Assert.That(errors.Length, Is.EqualTo(0));
    }

    private static JobRegistrationBuilder1 BuildJobs(JobRegistrationBuilder jobs) =>
        jobs
            .AddJob(DemoJobs.DoSomething.JobKey)
            .AddTasks(DemoJobs.DoSomething.GetAllTasks())
            .AddJob(DemoJobs.DoSomethingElse.JobKey)
            .AddTasks(DemoJobs.DoSomethingElse.GetAllTasks());

    [Test]
    public async Task ShouldNotTriggerJob_WhenNotSubscribedToTheEvent()
    {
        var host = TestHost.CreateDefault();
        await host.Register
        (
            events => events
                .AddEvent(DemoEventKeys.SomethingHappened)
                .AddEvent(DemoEventKeys.SomethingElseHappened),
            jobs => BuildJobs(jobs)
        );
        var eventNotifications = await host.RaiseEvent
        (
            DemoEventKeys.SomethingElseHappened,
            new XtiEventSource("1", "{ \"ID\": 1 }")
        );
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
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
        await host.Register
        (
            events => events.AddEvent(DemoEventKeys.SomethingHappened),
            jobs => BuildJobs(jobs)
        );
        var eventNotifications = await host.RaiseEvent
        (
            DemoEventKeys.SomethingHappened,
            new XtiEventSource("1", "{ \"ID\": 1 }")
        );
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
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
        await host.Register
        (
            events => events
                .AddEvent(DemoEventKeys.SomethingHappened)
                .IgnoreDuplicates().WhenSourceKeysAndDataAreEqual(),
            jobs => BuildJobs(jobs)
        );
        var eventNotifications1 = await host.RaiseEvent
        (
            DemoEventKeys.SomethingHappened,
            new XtiEventSource("1", "{ \"ID\": 1 }")
        );
        var eventNotifications2 = await host.RaiseEvent
        (
            DemoEventKeys.SomethingHappened,
            new XtiEventSource("1", "{ \"ID\": 1 }")
        );
        Assert.That(eventNotifications2.Length, Is.EqualTo(0), "Should ignore duplicate events.");
    }

    [Test]
    public async Task ShouldKeepAllDuplicateEvents()
    {
        var host = TestHost.CreateDefault();
        await host.Register
        (
            events => events
                .AddEvent(DemoEventKeys.SomethingHappened)
                .KeepAll(),
            jobs => BuildJobs(jobs)
        );
        var eventNotifications1 = await host.RaiseEvent
        (
            DemoEventKeys.SomethingHappened,
            new XtiEventSource("1", "{ \"ID\": 1 }")
        );
        var eventNotifications2 = await host.RaiseEvent
        (
            DemoEventKeys.SomethingHappened,
            new XtiEventSource("1", "{ \"ID\": 1 }")
        );
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
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
        await host.Register
        (
            events => events
                .AddEvent(DemoEventKeys.SomethingHappened)
                .KeepOldest().WhenSourceKeysAndDataAreEqual(),
            jobs => BuildJobs(jobs)
        );
        var eventNotifications1 = await host.RaiseEvent
        (
            DemoEventKeys.SomethingHappened,
            new XtiEventSource("1", "{ \"ID\": 1 }")
        );
        var eventNotifications2 = await host.RaiseEvent
        (
            DemoEventKeys.SomethingHappened,
            new XtiEventSource("1", "{ \"ID\": 1 }")
        );
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
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
        await host.Register
        (
            events => events
                .AddEvent(DemoEventKeys.SomethingHappened)
                .KeepNewest().WhenSourceKeysAndDataAreEqual(),
            jobs => BuildJobs(jobs)
        );
        var eventNotifications1 = await host.RaiseEvent
        (
            DemoEventKeys.SomethingHappened,
            new XtiEventSource("1", "{ \"ID\": 1 }")
        );
        var eventNotifications2 = await host.RaiseEvent
        (
            DemoEventKeys.SomethingHappened,
            new XtiEventSource("1", "{ \"ID\": 1 }")
        );
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
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
        await host.Register
        (
            events => events
                .AddEvent(DemoEventKeys.SomethingHappened)
                .IgnoreDuplicates().WhenSourceKeysOnlyAreEqual(),
            jobs => BuildJobs(jobs)
        );
        var eventNotifications1 = await host.RaiseEvent
        (
            DemoEventKeys.SomethingHappened,
            new XtiEventSource("1", "{ \"ID\": 1, \"Description\": \"Whatever\" }")
        );
        var eventNotifications2 = await host.RaiseEvent
        (
            DemoEventKeys.SomethingHappened,
            new XtiEventSource("1", "{ \"ID\": 1, \"Description\": \"Different\" }")
        );
        Assert.That(eventNotifications2.Length, Is.EqualTo(0), "Should handle events as duplicates when source keys only are equal");
    }

    [Test]
    public async Task ShouldNotTriggerJob_WhenEventIsNoLongerActive()
    {
        var host = TestHost.CreateDefault();
        var activeFor = TimeSpan.FromMinutes(5);
        await host.Register
        (
            events => events
                .AddEvent(DemoEventKeys.SomethingHappened)
                .IgnoreDuplicates().WhenSourceKeysOnlyAreEqual()
                .ActiveFor(activeFor),
            jobs => BuildJobs(jobs)
        );
        var clock = host.GetRequiredService<FakeClock>();
        var eventNotifications = await host.RaiseEvent
        (
            DemoEventKeys.SomethingHappened,
            new XtiEventSource("1", "{ \"ID\": 1 }")
        );
        clock.Add(activeFor.Add(TimeSpan.FromSeconds(1)));
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        var triggeredJobs = await eventNotifications[0].TriggeredJobs();
        Assert.That(triggeredJobs.Length, Is.EqualTo(0), "Should not trigger job when event is no longer active");
    }

    [Test]
    public async Task ShouldNotNotifyBeforeEventHasStarted()
    {
        var host = TestHost.CreateDefault();
        var clock = host.GetRequiredService<FakeClock>();
        await host.Register
        (
            events => events
                .AddEvent(DemoEventKeys.SomethingHappened)
                .IgnoreDuplicates().WhenSourceKeysOnlyAreEqual()
                .StartNotifying(clock.Now().AddMinutes(5)),
            jobs => BuildJobs(jobs)
        );
        var eventNotifications = await host.RaiseEvent
        (
            DemoEventKeys.SomethingHappened,
            new XtiEventSource("1", "{ \"ID\": 1 }")
        );
        Assert.That(eventNotifications.Length, Is.EqualTo(0), "Should not notify before event has started.");
    }

    [Test]
    public async Task ShouldNotTriggerJob_WhenEventWasRaisedBeforeEventStartTime()
    {
        var host = TestHost.CreateDefault();
        await host.Register
        (
            events => events.AddEvent(DemoEventKeys.SomethingHappened),
            jobs => BuildJobs(jobs)
        );
        var sourceData = new SomethingHappenedData
        {
            ID = 2,
            Items = Enumerable.Range(1, 3).ToArray()
        };
        var eventNotifications1 = await host.RaiseEvent
        (
            DemoEventKeys.SomethingHappened,
            new XtiEventSource(sourceData.ID.ToString(), JsonSerializer.Serialize(sourceData))
        );
        var now = host.CurrentTime();
        var eventRaisedStartTime = now.AddMinutes(5);
        await host.MonitorEvent
        (
            DemoEventKeys.SomethingHappened,
            DemoJobs.DoSomething.JobKey,
            (monitorBuilder) =>
            {
                monitorBuilder.HandleEventsRaisedOnOrAfter(eventRaisedStartTime);
            }
        );
        var triggeredJobs1 = await eventNotifications1[0].TriggeredJobs();
        Assert.That(triggeredJobs1.Length, Is.EqualTo(0), "Should not trigger jobs before event raised start time");
        host.FastForward(TimeSpan.FromMinutes(5).Add(TimeSpan.FromSeconds(1)));
        sourceData.ID = 4;
        var eventNotifications2 = await host.RaiseEvent
        (
            DemoEventKeys.SomethingHappened,
            new XtiEventSource(sourceData.ID.ToString(), JsonSerializer.Serialize(sourceData))
        );
        await host.MonitorEvent
        (
            DemoEventKeys.SomethingHappened,
            DemoJobs.DoSomething.JobKey,
            (monitorBuilder) =>
            {
                monitorBuilder.HandleEventsRaisedOnOrAfter(eventRaisedStartTime);
            }
        );
        var triggeredJobs2 = await eventNotifications2[0].TriggeredJobs();
        Assert.That(triggeredJobs2.Length, Is.EqualTo(1), "Should trigger jobs after event raised start time");
    }

    [Test]
    public async Task ShouldNotifyAboutEventsFromMultipleSources()
    {
        var host = TestHost.CreateDefault();
        await host.Register
        (
            events => events.AddEvent(DemoEventKeys.SomethingHappened),
            jobs => jobs.BuildJobs()
        );
        var sourceData1 = new SomethingHappenedData
        {
            ID = 2,
            Items = Enumerable.Range(1, 3).ToArray()
        };
        var sourceData2 = new SomethingHappenedData
        {
            ID = 3,
            Items = Enumerable.Range(4, 3).ToArray()
        };
        var eventNotifications = await host.RaiseEvent
        (
            DemoEventKeys.SomethingHappened,
            new XtiEventSource(sourceData1.ID.ToString(), JsonSerializer.Serialize(sourceData1)),
            new XtiEventSource(sourceData2.ID.ToString(), JsonSerializer.Serialize(sourceData2))
        );
        Assert.That
        (
            eventNotifications.Select(n => n.ToModel().SourceKey),
            Has.One.EqualTo("2"),
            "Should notify event with source key '2'"
        );
        Assert.That
        (
            eventNotifications.Select(n => n.ToModel().SourceKey),
            Has.One.EqualTo("3"),
            "Should notify event with source key '3'"
        );
    }
}
