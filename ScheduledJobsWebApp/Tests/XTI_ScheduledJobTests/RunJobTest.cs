﻿using System;
using System.Linq;
using System.Text.Json;
using XTI_Core.Extensions;
using XTI_Core.Fakes;

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
    public async Task ShouldCompleteJob()
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
        var triggeredJobs = await eventNotifications[0].TriggeredJobs();
        var status = triggeredJobs[0].Status();
        Assert.That(status, Is.EqualTo(JobTaskStatus.Values.Completed), "Should complete job");
    }

    [Test]
    public async Task ShouldLogMessage()
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
        var context = host.GetRequiredService<DemoActionContext<DemoAction01>>();
        context.Messages = new[] { "Whatever" };
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        var triggeredJobs = await eventNotifications[0].TriggeredJobs();
        var messages = triggeredJobs[0].Messages();
        Assert.That(messages.Length, Is.EqualTo(1), "Should log message");
        Assert.That(messages[0].Message, Is.EqualTo("Whatever"), "Should log message");
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

    [Test]
    public async Task ShouldFailJob_WhenErrorOccurs()
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
        var status = triggeredJobs[0].Status();
        Assert.That(status, Is.EqualTo(JobTaskStatus.Values.Failed), "Should fail job when task fails");
    }

    [Test]
    public async Task ShouldRunNextChildTask()
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
        var demoContext1 = host.GetRequiredService<DemoItemActionContext<DemoItem01Action>>();
        demoContext1.ThrowErrorWhen("Whatever", data => data.ItemID == 2);
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        var demoContext2 = host.GetRequiredService<DemoItemActionContext<DemoItem02Action>>();
        var values = demoContext2.Values();
        Assert.That(values, Is.EqualTo(new[] { "Value1" }), "Should run next child task");
    }

    [Test]
    public async Task ShouldNotContinueAfterJobFails()
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
        var values = demoContext.Values();
        Assert.That(values, Is.EqualTo(new[] { "Value1" }));
    }

    [Test]
    public async Task ShouldCancelJob_WhenJobIsCanceled()
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
        demoContext.CancelAfterError();
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        var triggeredJobs = await eventNotifications[0].TriggeredJobs();
        var status = triggeredJobs[0].Status();
        Assert.That(status, Is.EqualTo(JobTaskStatus.Values.Canceled), "Should cancel job");
    }

    [Test]
    public async Task ShouldNotContinue_WhenJobIsCanceled()
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
        demoContext.CancelAfterError();
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        var values = demoContext.Values();
        Assert.That(values, Is.EqualTo(new[] { "Value1" }));
    }

    [Test]
    public async Task ShouldContinueJobAfterError()
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
        demoContext.ContinueAfterError();
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        var values = demoContext.Values();
        Assert.That(values, Is.EqualTo(new[] { "Value1", "Value3" }));
    }

    [Test]
    public async Task ShouldContinueNextTaskAfterError()
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
        var demoContext01 = host.GetRequiredService<DemoItemActionContext<DemoItem01Action>>();
        demoContext01.ThrowErrorWhen("Whatever", data => data.ItemID == 2);
        demoContext01.ContinueAfterError();
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        var demoContext02 = host.GetRequiredService<DemoItemActionContext<DemoItem02Action>>();
        var values = demoContext02.Values();
        Assert.That(values, Is.EqualTo(new[] { "Value1", "Value2", "Value3" }));
    }

    [Test]
    public async Task ShouldCompleteJob_WhenContinuingAfterError()
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
        var demoContext01 = host.GetRequiredService<DemoItemActionContext<DemoItem01Action>>();
        demoContext01.ThrowErrorWhen("Whatever", data => data.ItemID == 2);
        demoContext01.ContinueAfterError();
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        var triggeredJobs = await eventNotifications[0].TriggeredJobs();
        var status = triggeredJobs[0].Status();
        Assert.That(status, Is.EqualTo(JobTaskStatus.Values.Completed), "Should complete job when continuing after an error");
    }

    [Test]
    public async Task ShouldNotContinueNextTask_WhenRetryingAfterError()
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
        var demoContext01 = host.GetRequiredService<DemoItemActionContext<DemoItem01Action>>();
        demoContext01.ThrowErrorWhen("Whatever", data => data.ItemID == 2);
        demoContext01.RetryAfterError(TimeSpan.FromMinutes(5));
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        var values = demoContext01.Values();
        Assert.That(values, Is.EqualTo(new[] { "Value1" }));
    }

    [Test]
    public async Task ShouldRedoTask_WhenRetryingAfterError()
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
        var demoContext01 = host.GetRequiredService<DemoItemActionContext<DemoItem01Action>>();
        demoContext01.ThrowErrorWhen("Whatever", data => data.ItemID == 2);
        demoContext01.RetryAfterError(TimeSpan.FromMinutes(5));
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        demoContext01.DontThrowError();
        var howLong = TimeSpan.FromMinutes(5).Add(TimeSpan.FromSeconds(1));
        fastForward(host, howLong);
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        var values = demoContext01.Values();
        Assert.That(values, Is.EqualTo(new[] { "Value1", "Value2", "Value3" }), "Should redo task when retrying after an error");
    }

    [Test]
    public async Task ShouldCompleteJob_WhenRetryingAfterError()
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
        var demoContext01 = host.GetRequiredService<DemoItemActionContext<DemoItem01Action>>();
        demoContext01.ThrowErrorWhen("Whatever", data => data.ItemID == 2);
        demoContext01.RetryAfterError(TimeSpan.FromMinutes(5));
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        demoContext01.DontThrowError();
        var howLong = TimeSpan.FromMinutes(5).Add(TimeSpan.FromSeconds(1));
        fastForward(host, howLong);
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        var triggeredJobs = await eventNotifications[0].TriggeredJobs();
        var status = triggeredJobs[0].Status();
        Assert.That(status, Is.EqualTo(JobTaskStatus.Values.Completed), "Should complete job when retrying after an error");
    }

    [Test]
    public async Task ShouldNotRedoTaskBeforeRetryTime()
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
        var demoContext01 = host.GetRequiredService<DemoItemActionContext<DemoItem01Action>>();
        demoContext01.ThrowErrorWhen("Whatever", data => data.ItemID == 2);
        demoContext01.RetryAfterError(TimeSpan.FromMinutes(5));
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        demoContext01.DontThrowError();
        fastForward(host, TimeSpan.FromMinutes(5).Add(TimeSpan.FromSeconds(-1)));
        await host.MonitorEvent(DemoEventKeys.SomethingHappened, DemoJobs.DoSomething.JobKey);
        var values = demoContext01.Values();
        Assert.That(values, Is.EqualTo(new[] { "Value1" }), "Should not redo task before retry time");
    }

    private static void fastForward(XtiHost host, TimeSpan howLong)
    {
        var clock = host.GetRequiredService<FakeClock>();
        clock.Add(howLong);
    }

    private static JobRegistration BuildJobs(JobRegistration jobs) =>
        jobs.AddJob
        (
            DemoJobs.DoSomething.JobKey,
            job => job
                .AddFirstTask(DemoJobs.DoSomething.Task01)
                .AddTask(DemoJobs.DoSomething.Task02)
                .AddTask(DemoJobs.DoSomething.TaskItem01)
                .AddTask(DemoJobs.DoSomething.TaskItem02)
        );
}