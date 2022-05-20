using System.Linq;
using System.Text.Json;

namespace XTI_ScheduledJobTests;

internal sealed class RunJobTest
{
    [Test]
    public async Task ShouldRunFirstJobTask()
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
            new EventSource("1", "{ \"ID\": 1 }")
        );
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        var action01Context = host.GetRequiredService<DemoActionContext<DemoAction01>>();
        Assert.That(action01Context.NumberOfTimesExecuted, Is.EqualTo(1), "Should run first job task");
    }

    [Test]
    public async Task ShouldRunNextJobTask()
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
            new EventSource("1", "{ \"ID\": 1 }")
        );
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        var action02Context = host.GetRequiredService<DemoActionContext<DemoAction02>>();
        Assert.That(action02Context.NumberOfTimesExecuted, Is.EqualTo(1), "Should run next job task");
    }

    [Test]
    public async Task ShouldTransformSourceDataAndPassToTasks()
    {
        var host = TestHost.CreateDefault();
        await host.Register
        (
            events => events.AddEvent(DemoEventKeys.SomethingHappened),
            jobs => BuildJobs(jobs)
        );
        var sourceData = new SomethingHappenedData { ID = 2 };
        var eventNotifications = await host.RaiseEvent
        (
            DemoEventKeys.SomethingHappened,
            new EventSource(sourceData.ID.ToString(), JsonSerializer.Serialize(sourceData))
        );
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        var demoContext = host.GetRequiredService<DemoActionContext<DemoAction02>>();
        Assert.That(demoContext.TargetID, Is.EqualTo(20), "Should transform source data and pass to tasks");
    }

    [Test]
    public async Task ShouldTransformDataAsTasksAreCompleted()
    {
        var host = TestHost.CreateDefault();
        await host.Register
        (
            events => events.AddEvent(DemoEventKeys.SomethingHappened),
            jobs => BuildJobs(jobs)
        );
        var sourceData = new SomethingHappenedData { ID = 2 };
        var eventNotifications = await host.RaiseEvent
        (
            DemoEventKeys.SomethingHappened,
            new EventSource(sourceData.ID.ToString(), JsonSerializer.Serialize(sourceData))
        );
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        var demoContext = host.GetRequiredService<DemoActionContext<DemoAction02>>();
        Assert.That(demoContext.Output, Is.EqualTo("Action1,Action2"), "Should transform data as tasks are completed");
    }

    [Test]
    public async Task ShouldLoopThroughTasks()
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
        var eventNotifications = await host.RaiseEvent
        (
            DemoEventKeys.SomethingHappened,
            new EventSource(sourceData.ID.ToString(), JsonSerializer.Serialize(sourceData))
        );
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        var demoContext = host.GetRequiredService<DemoItemActionContext<DemoItem01Action>>();
        Assert.That(demoContext.Values, Is.EqualTo(new[] { "Value1", "Value2", "Value3" }), "Should loop through tasks");
    }

    [Test]
    public async Task ShouldLogError()
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
        var eventNotifications = await host.RaiseEvent
        (
            DemoEventKeys.SomethingHappened,
            new EventSource(sourceData.ID.ToString(), JsonSerializer.Serialize(sourceData))
        );
        var demoContext = host.GetRequiredService<DemoItemActionContext<DemoItem01Action>>();
        demoContext.ThrowErrorWhen("Whatever", data => data.ItemID == 2);
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        var triggeredJobs = await eventNotifications[0].TriggeredJobs();
        var errors = triggeredJobs[0].Errors();
        Assert.That(errors.Length, Is.EqualTo(1), "Should log error");
        Assert.That(errors[0].Message, Is.EqualTo("Whatever"), "Should log error");
        Assert.That(errors[0].Category, Is.EqualTo("DemoItemActionException"), "Should log error");
    }

    private static JobRegistration BuildJobs(JobRegistration jobs) =>
        jobs.AddJob
        (
            DemoJobs.DoSomething.JobKey,
            job => job
                .AddFirstTask(DemoJobs.DoSomething.Task01)
                .AddTask(DemoJobs.DoSomething.Task02)
                .AddTask(DemoJobs.DoSomething.TaskItem)
        );
}
