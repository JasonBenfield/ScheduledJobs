using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XTI_App.Api;
using XTI_ScheduledJobsWebAppApi;
using XTI_ScheduledJobsWebAppApi.Tasks;

namespace XTI_ScheduledJobTests;

internal sealed class TimeoutTaskTest
{
    [Test]
    public async Task ShouldTimeoutTask()
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
        var demoContext01 = host.GetRequiredService<DemoActionContext<DemoAction01>>();
        demoContext01.Delay = TimeSpan.FromHours(1);
        var monitor = Task.Run
        (
            () => host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey)
        );
        await Task.Delay(TimeSpan.FromSeconds(1));
        var api = host.GetRequiredService<ScheduledJobsAppApi>();
        var beforeTriggeredJobs = await eventNotifications[0].TriggeredJobs();
        var runningTask = beforeTriggeredJobs[0].Tasks()
            .First(t => t.Model.Status.Equals(JobTaskStatus.Values.Running));
        await api.Tasks.TimeoutTask.Invoke(new GetTaskRequest(runningTask.Model.ID));
        var afterTriggeredJobs = await eventNotifications[0].TriggeredJobs();
        Assert.That
        (
            afterTriggeredJobs[0].Status(),
            Is.EqualTo(JobTaskStatus.Values.Failed),
            "Should timeout task"
        );
        host.GetRequiredService<CancellationTokenSource>().Cancel();
    }

    [Test]
    public async Task ShouldNotTimeoutCompletedTask()
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
            () => api.Tasks.TimeoutTask.Invoke(new GetTaskRequest(completedTask.Model.ID))
        );
        Assert.That
        (
            ex.Message,
            Is.EqualTo
            (
                string.Format(TaskErrors.TaskWithStatusCannotBeTimedOut, JobTaskStatus.Values.Completed.DisplayText)
            )
        );
    }

    [Test]
    public async Task ShouldLogError_WhenTaskTimesOut()
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
        var demoContext01 = host.GetRequiredService<DemoActionContext<DemoAction01>>();
        demoContext01.Delay = TimeSpan.FromHours(1);
        var monitor = Task.Run
        (
            () => host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey)
        );
        await Task.Delay(TimeSpan.FromSeconds(1));
        var api = host.GetRequiredService<ScheduledJobsAppApi>();
        var beforeTriggeredJobs = await eventNotifications[0].TriggeredJobs();
        var runningTask = beforeTriggeredJobs[0].Tasks()
            .First(t => t.Model.Status.Equals(JobTaskStatus.Values.Running));
        await api.Tasks.TimeoutTask.Invoke(new GetTaskRequest(runningTask.Model.ID));
        var afterTriggeredJobs = await eventNotifications[0].TriggeredJobs();
        var errors = afterTriggeredJobs[0].Errors();
        Assert.That(errors.Length, Is.EqualTo(1), "Should log error when task times out");
        Assert.That(errors[0].Category, Is.EqualTo(JobErrors.TaskTimeoutCategory), "Should log error when task times out");
        Assert.That(errors[0].Message, Is.EqualTo(JobErrors.TaskTimeoutMessage), "Should log error when task times out");
        host.GetRequiredService<CancellationTokenSource>().Cancel();
    }
}
