using System.Text.Json;
using XTI_Core;
using XTI_JobsDB.EF;

namespace XTI_ScheduledJobIntegrationTests;

internal sealed class RunJobTest
{
    [Test]
    public async Task ShouldRunJob()
    {
        var host = TestHost.CreateDefault(XtiEnvironment.Development);
        await host.Setup();
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
        var eventNotifications = await host.RaiseEvent
        (
            DemoEventKeys.SomethingHappened,
            new EventSource(sourceData.ID.ToString(), JsonSerializer.Serialize(sourceData))
        );
        Assert.That(eventNotifications.Length, Is.EqualTo(1), "Should raise event");
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        var jobs = await eventNotifications[0].TriggeredJobs();
        Assert.That(jobs.Length, Is.EqualTo(1), "Should trigger job");
        Assert.That(jobs[0].Status(), Is.EqualTo(JobTaskStatus.Values.Completed), "Should complete job");
    }

    [Test]
    public async Task ShouldFailJob_WhenErrorOccurs()
    {
        var host = TestHost.CreateDefault(XtiEnvironment.Development);
        await host.Setup();
        await host.Register
        (
            events => events.AddEvent(DemoEventKeys.SomethingHappened),
            jobs => BuildJobs(jobs)
        );
        var sourceData = new SomethingHappenedData
        {
            ID = 7,
            Items = Enumerable.Range(1, 3).ToArray()
        };
        var eventNotifications = await host.RaiseEvent
        (
            DemoEventKeys.SomethingHappened,
            new EventSource(sourceData.ID.ToString(), JsonSerializer.Serialize(sourceData))
        );
        var demoContext = host.GetRequiredService<DemoItemActionContext<DemoItemAction01>>();
        demoContext.ThrowErrorWhen("Whatever", data => data.ItemID == 2);
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        var jobs = await eventNotifications[0].TriggeredJobs();
        Assert.That(jobs[0].Status(), Is.EqualTo(JobTaskStatus.Values.Failed), "Should fail job");
        var errors = jobs[0].Errors();
        Assert.That(errors.Select(err => err.Category), Is.EqualTo(new[] { "DemoItemActionException" }), "Should log errors");
        Assert.That(errors.Select(err => err.Message), Is.EqualTo(new[] { "Whatever" }), "Should log errors");
    }

    [Test]
    public async Task ShouldRetryJob()
    {
        var host = TestHost.CreateDefault();
        await host.Setup();
        await host.Register
        (
            events => events.AddEvent(DemoEventKeys.SomethingHappened),
            jobs => BuildJobs(jobs)
        );
        var sourceData = new SomethingHappenedData
        {
            ID = 4,
            Items = Enumerable.Range(1, 3).ToArray()
        };
        var eventNotifications = await host.RaiseEvent
        (
            DemoEventKeys.SomethingHappened,
            new EventSource(sourceData.ID.ToString(), JsonSerializer.Serialize(sourceData))
        );
        var demoContext = host.GetRequiredService<DemoItemActionContext<DemoItemAction01>>();
        demoContext.ThrowErrorWhen("Whatever", data => data.ItemID == 2);
        demoContext.RetryAfterError(TimeSpan.Zero);
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        demoContext.DontThrowError();
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        var jobs = await eventNotifications[0].TriggeredJobs();
        Assert.That(jobs[0].Status(), Is.EqualTo(JobTaskStatus.Values.Completed), "Should retry job");
    }

    [Test]
    public async Task ShouldRetryAfterErrorDuringTransformSourceData()
    {
        var host1 = TestHost.CreateDefault(XtiEnvironment.Development);
        var db = host1.GetRequiredService<JobDbContext>();
        await host1.Setup();
        await host1.Register
        (
            events => events.AddEvent(DemoEventKeys.SomethingHappened),
            jobs => BuildJobs(jobs)
        );
        var sourceData = new SomethingHappenedData
        {
            ID = 16,
            Items = Enumerable.Range(1, 3).ToArray()
        };
        var eventNotifications = await host1.RaiseEvent
        (
            DemoEventKeys.SomethingHappened,
            new EventSource(sourceData.ID.ToString(), JsonSerializer.Serialize(sourceData))
        );
        var actionFactory = host1.GetRequiredService<DemoJobActionFactory>();
        actionFactory.FailTransformSourceData();
        try
        {
            await host1.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        }
        catch { }
        actionFactory.AllowTransformSourceData();
        var host2 = TestHost.CreateDefault(XtiEnvironment.Development);
        await host2.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        var triggeredJobs = await eventNotifications[0].TriggeredJobs();
        Assert.That(triggeredJobs[0].Status(), Is.EqualTo(JobTaskStatus.Values.Completed), "Should retry after error during transform source data");
    }

    private static JobRegistration BuildJobs(JobRegistration jobs) =>
        jobs.AddJob
        (
            DemoJobs.DoSomething.JobKey,
            job => job
                .TimeoutAfter(TimeSpan.FromHours(1))
                .AddTask(DemoJobs.DoSomething.Task01).TimeoutAfter(TimeSpan.FromMinutes(5))
                .AddTask(DemoJobs.DoSomething.Task02).TimeoutAfter(TimeSpan.FromMinutes(5))
                .AddTask(DemoJobs.DoSomething.TaskItem01).TimeoutAfter(TimeSpan.FromMinutes(5))
                .AddTask(DemoJobs.DoSomething.TaskItem02).TimeoutAfter(TimeSpan.FromMinutes(5))
                .AddTask(DemoJobs.DoSomething.TaskFinal).TimeoutAfter(TimeSpan.FromMinutes(5))
        );
}
