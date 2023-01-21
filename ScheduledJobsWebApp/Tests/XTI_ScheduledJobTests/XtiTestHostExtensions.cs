using System;
using XTI_Core;
using XTI_Core.Extensions;
using XTI_Core.Fakes;

namespace XTI_ScheduledJobTests;

internal static class XtiTestHostExtensions
{
    public static async Task Register(this XtiHost host, Action<EventRegistration> configEvents, Action<JobRegistration> configJobs)
    {
        var events = host.GetRequiredService<EventRegistration>();
        configEvents(events);
        await events.Register();
        var jobs = host.GetRequiredService<JobRegistration>();
        configJobs(jobs);
        await jobs.Register();
    }

    public static Task MonitorEvent(this XtiHost host, EventKey eventKey, JobKey jobKey, Action<EventMonitorBuilder1>? configMonitor = null)
    {
        var stoppingToken = host.GetRequiredService<CancellationTokenSource>().Token;
        var monitorBuilder = host.GetRequiredService<EventMonitorBuilder>();
        var monitorBuilder1 = monitorBuilder.When(eventKey);
        configMonitor?.Invoke(monitorBuilder1);
        var jobActionFactory = host.GetRequiredService<DemoJobActionFactory>();
        var transformedEventData = host.GetRequiredService<DemoTransformedEventData>();
        var monitor = monitorBuilder1
            .Trigger(jobKey)
            .UseJobActionFactory(jobActionFactory)
            .TransformEventData(transformedEventData);
        return monitor.Run(stoppingToken);
    }

    public static Task<EventNotification[]> RaiseEvent
    (
        this XtiHost host, 
        EventKey eventKey, 
        params XtiEventSource[] sources
    )
    {
        var incomingEventFactory = host.GetRequiredService<IncomingEventFactory>();
        return incomingEventFactory
            .Incoming(eventKey)
            .From(sources)
            .Notify();
    }

    public static void FastForward(this XtiHost host, TimeSpan howLong)
    {
        var clock = host.GetRequiredService<FakeClock>();
        clock.Add(howLong);
    }

    public static DateTimeOffset CurrentTime(this XtiHost host) =>
        host.GetRequiredService<IClock>().Now();

    public static JobRegistration BuildJobs(this JobRegistration jobs) =>
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
