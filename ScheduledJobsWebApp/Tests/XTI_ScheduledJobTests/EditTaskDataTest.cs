using XTI_App.Api;
using XTI_Core;
using XTI_ScheduledJobsWebAppApi;
using XTI_ScheduledJobsWebAppApi.Tasks;

namespace XTI_ScheduledJobTests;

internal sealed class EditTaskDataTest
{
    [Test]
    public async Task ShouldEditTaskData()
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
        var beforeTriggeredJobs = await eventNotifications[0].TriggeredJobs();
        var api = host.GetRequiredService<ScheduledJobsAppApi>();
        var beforeFailedTask = beforeTriggeredJobs[0].Tasks()
            .First(t => t.Model.Status.Equals(JobTaskStatus.Values.Failed));
        var beforeTaskData = XtiSerializer.Deserialize<DoSomethingItemData>(beforeFailedTask.Model.TaskData);
        beforeTaskData.AnotherValue = "Changed Task Data";
        await api.Tasks.EditTaskData.Invoke
        (
            new EditTaskDataRequest
            {
                TaskID = beforeFailedTask.Model.ID,
                TaskData = XtiSerializer.Serialize(beforeTaskData)
            }
        );
        var afterTriggeredJobs = await eventNotifications[0].TriggeredJobs();
        var afterFailedTask = afterTriggeredJobs[0].Tasks()
            .First(t => t.Model.Status.Equals(JobTaskStatus.Values.Failed));
        var afterTaskData = XtiSerializer.Deserialize<DoSomethingItemData>(afterFailedTask.Model.TaskData);
        Assert.That(afterTaskData.AnotherValue, Is.EqualTo("Changed Task Data"), "Should edit task data");
    }

    [Test]
    public async Task ShouldNotEditTaskData_WhenTaskDataDidNotChange()
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
        var beforeTriggeredJobs = await eventNotifications[0].TriggeredJobs();
        var api = host.GetRequiredService<ScheduledJobsAppApi>();
        var beforeFailedTask = beforeTriggeredJobs[0].Tasks()
            .First(t => t.Model.Status.Equals(JobTaskStatus.Values.Failed));
        var beforeTaskData = XtiSerializer.Deserialize<DoSomethingItemData>(beforeFailedTask.Model.TaskData);
        await api.Tasks.EditTaskData.Invoke
        (
            new EditTaskDataRequest
            {
                TaskID = beforeFailedTask.Model.ID,
                TaskData = beforeFailedTask.Model.TaskData
            }
        );
        var afterTriggeredJobs = await eventNotifications[0].TriggeredJobs();
        var messages = afterTriggeredJobs[0].Messages();
        Assert.That(messages.Length, Is.EqualTo(0), "Should not edit task data when nothing changed");
    }

    [Test]
    public async Task ShouldLogOriginalTaskData()
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
        var beforeTriggeredJobs = await eventNotifications[0].TriggeredJobs();
        var api = host.GetRequiredService<ScheduledJobsAppApi>();
        var beforeFailedTask = beforeTriggeredJobs[0].Tasks()
            .First(t => t.Model.Status.Equals(JobTaskStatus.Values.Failed));
        var beforeTaskData = XtiSerializer.Deserialize<DoSomethingItemData>(beforeFailedTask.Model.TaskData);
        beforeTaskData.AnotherValue = "Changed Task Data";
        await api.Tasks.EditTaskData.Invoke
        (
            new EditTaskDataRequest
            {
                TaskID = beforeFailedTask.Model.ID,
                TaskData = XtiSerializer.Serialize(beforeTaskData)
            }
        );
        var afterTriggeredJobs = await eventNotifications[0].TriggeredJobs();
        var messages = afterTriggeredJobs[0].Messages();
        Assert.That(messages.Select(m => m.Category), Is.EqualTo(new[] { "OriginalTaskData" }));
        Assert.That(messages.Select(m => m.Message), Is.EqualTo(new[] { beforeFailedTask.Model.TaskData }));
    }

    [Test]
    public async Task ShouldEditTaskData_WhenTaskHasBeenCompleted()
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
            .First
            (
                t => t.Model.Status.Equals(JobTaskStatus.Values.Completed) && 
                    t.Model.TaskDefinition.TaskKey.Equals(DemoJobs.DoSomething.TaskItem01)
            );
        var beforeTaskData = XtiSerializer.Deserialize<DoSomethingItemData>(completedTask.Model.TaskData);
        beforeTaskData.AnotherValue = "Changed Task Data";
        var ex = Assert.ThrowsAsync<AppException>
        (
            () => api.Tasks.EditTaskData.Invoke
            (
                new EditTaskDataRequest
                {
                    TaskID = completedTask.Model.ID,
                    TaskData = XtiSerializer.Serialize(beforeTaskData)
                }
            )
        );
        Assert.That
        (
            ex.Message,
            Is.EqualTo
            (
                string.Format(TaskErrors.TaskWithStatusCannotEditTaskData, JobTaskStatus.Values.Completed.DisplayText)
            )
        );
    }
}
