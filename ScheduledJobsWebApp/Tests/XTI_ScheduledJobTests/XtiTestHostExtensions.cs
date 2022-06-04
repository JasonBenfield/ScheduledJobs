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
        var jobActionFactory = host.GetRequiredService<IJobActionFactory>();
        var monitor = monitorBuilder1
            .Trigger(jobKey)
            .UseJobActionFactory(jobActionFactory);
        return monitor.Run(stoppingToken);
    }

    public static Task<EventNotification[]> RaiseEvent(this XtiHost host, EventKey eventKey, EventSource source)
    {
        var incomingEventFactory = host.GetRequiredService<IncomingEventFactory>();
        return incomingEventFactory
            .Incoming(eventKey)
            .From(source)
            .Notify();
    }

    public static void FastForward(this XtiHost host, TimeSpan howLong)
    {
        var clock = host.GetRequiredService<FakeClock>();
        clock.Add(howLong);
    }

    public static DateTimeOffset CurrentTime(this XtiHost host) =>
        host.GetRequiredService<IClock>().Now();
}
