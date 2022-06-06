using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XTI_App.Api;
using XTI_ScheduledJobsWebAppApi;
using XTI_ScheduledJobsWebAppApi.Tasks;

namespace XTI_ScheduledJobTests;

internal sealed class RetryTaskTest
{
    [Test]
    public async Task ShouldRetryTask()
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
        demoContext.ThrowErrorWhen("Whatever", data => data.ItemID == 2);
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        var triggeredJobs = await eventNotifications[0].TriggeredJobs();
        var api = host.GetRequiredService<ScheduledJobsAppApi>();
        var failedTask = triggeredJobs[0].Tasks()
            .First(t => t.Model.Status.Equals(JobTaskStatus.Values.Failed));
        await api.Tasks.RetryTask.Invoke(new GetTaskRequest(failedTask.Model.ID));
        triggeredJobs = await eventNotifications[0].TriggeredJobs();
        Assert.That
        (
            triggeredJobs[0].Status(),
            Is.EqualTo(JobTaskStatus.Values.Retry),
            "Should retry job"
        );
    }

    [Test]
    public async Task ShouldNotRetryCompletedTask()
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
        demoContext.ThrowErrorWhen("Whatever", data => data.ItemID == 2);
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        var triggeredJobs = await eventNotifications[0].TriggeredJobs();
        var api = host.GetRequiredService<ScheduledJobsAppApi>();
        var completedTask = triggeredJobs[0].Tasks()
            .First(t => t.Model.Status.Equals(JobTaskStatus.Values.Completed));
        var ex = Assert.ThrowsAsync<AppException>
        (
            () => api.Tasks.RetryTask.Invoke(new GetTaskRequest(completedTask.Model.ID))
        );
        Assert.That
        (
            ex.Message,
            Is.EqualTo
            (
                string.Format(TaskErrors.TaskWithStatusCannotBeRetried, JobTaskStatus.Values.Completed.DisplayText)
            )
        );
    }
}
