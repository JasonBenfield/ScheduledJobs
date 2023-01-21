using XTI_App.Api;
using XTI_ScheduledJobsWebAppApi;
using XTI_ScheduledJobsWebAppApi.Tasks;

namespace XTI_ScheduledJobTests;

internal sealed class SkipTaskTest
{
    [Test]
    public async Task ShouldSkipTask()
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
            new XtiEventSource(sourceData.ID.ToString(), JsonSerializer.Serialize(sourceData))
        );
        var demoContext = host.GetRequiredService<DemoItemActionContext<DemoItemAction01>>();
        demoContext.ThrowErrorWhen("Whatever", data => data.ItemID == 2);
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        var triggeredJobs = await eventNotifications[0].TriggeredJobs();
        var api = host.GetRequiredService<ScheduledJobsAppApi>();
        var failedTask = triggeredJobs[0].Tasks()
            .First(t => t.Model.Status.Equals(JobTaskStatus.Values.Failed));
        await api.Tasks.SkipTask.Invoke(new GetTaskRequest(failedTask.Model.ID));
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        triggeredJobs = await eventNotifications[0].TriggeredJobs();
        Assert.That
        (
            triggeredJobs[0].Status(),
            Is.EqualTo(JobTaskStatus.Values.Completed),
            "Should complete job"
        );
    }

    [Test]
    public async Task ShouldLogSkippedMessage()
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
            new XtiEventSource(sourceData.ID.ToString(), JsonSerializer.Serialize(sourceData))
        );
        var demoContext = host.GetRequiredService<DemoItemActionContext<DemoItemAction01>>();
        demoContext.ThrowErrorWhen("Whatever", data => data.ItemID == 2);
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        var triggeredJobs = await eventNotifications[0].TriggeredJobs();
        var api = host.GetRequiredService<ScheduledJobsAppApi>();
        var failedTask = triggeredJobs[0].Tasks()
            .First(t => t.Model.Status.Equals(JobTaskStatus.Values.Failed));
        await api.Tasks.SkipTask.Invoke(new GetTaskRequest(failedTask.Model.ID));
        triggeredJobs = await eventNotifications[0].TriggeredJobs();
        var messages = triggeredJobs[0].Messages();
        Assert.That(messages.Length, Is.EqualTo(1), "Should log message after skipped");
        Assert.That(messages[0].Category, Is.EqualTo(JobErrors.TaskSkippedCategory), "Should log message after skipped");
        Assert.That(messages[0].Message, Is.EqualTo(JobErrors.TaskSkippedMessage), "Should log message after skipped");
    }

    [Test]
    public async Task ShouldPreserveData()
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
            new XtiEventSource(sourceData.ID.ToString(), JsonSerializer.Serialize(sourceData))
        );
        var demoContext = host.GetRequiredService<DemoItemActionContext<DemoItemAction01>>();
        demoContext.ThrowErrorWhen("Whatever", data => data.ItemID == 2);
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        var triggeredJobs = await eventNotifications[0].TriggeredJobs();
        var api = host.GetRequiredService<ScheduledJobsAppApi>();
        var failedTask = triggeredJobs[0].Tasks()
            .First(t => t.Model.Status.Equals(JobTaskStatus.Values.Failed));
        await api.Tasks.SkipTask.Invoke(new GetTaskRequest(failedTask.Model.ID));
        triggeredJobs = await eventNotifications[0].TriggeredJobs();
        var task = triggeredJobs[0].Tasks(DemoJobs.DoSomething.TaskItem01)[1];
        Assert.That(task.Data<DoSomethingItemData>().ItemID, Is.EqualTo(2), "Should preserve data");
    }

    [Test]
    public async Task ShouldNotSkipCompletedTask()
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
            new XtiEventSource(sourceData.ID.ToString(), JsonSerializer.Serialize(sourceData))
        );
        var demoContext = host.GetRequiredService<DemoItemActionContext<DemoItemAction01>>();
        demoContext.ThrowErrorWhen("Whatever", data => data.ItemID == 2);
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        var triggeredJobs = await eventNotifications[0].TriggeredJobs();
        var api = host.GetRequiredService<ScheduledJobsAppApi>();
        var completedTask = triggeredJobs[0].Tasks()
            .First(t => t.Model.Status.Equals(JobTaskStatus.Values.Completed));
        var ex = Assert.ThrowsAsync<AppException>
        (
            () => api.Tasks.SkipTask.Invoke(new GetTaskRequest(completedTask.Model.ID))
        );
        Assert.That
        (
            ex.Message,
            Is.EqualTo
            (
                string.Format(TaskErrors.TaskWithStatusCannotBeSkipped, JobTaskStatus.Values.Completed.DisplayText)
            )
        );
    }
}
