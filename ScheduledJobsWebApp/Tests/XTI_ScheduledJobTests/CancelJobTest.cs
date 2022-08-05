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
}
