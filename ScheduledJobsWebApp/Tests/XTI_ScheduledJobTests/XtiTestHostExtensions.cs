using System;
using XTI_Core.Extensions;

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

    public static Task MonitorEvent(this XtiHost host, EventKey eventKey, JobKey jobKey, Action<EventMonitorBuilder>? configMonitor =null)
    {
        var stoppingToken = host.GetRequiredService<CancellationTokenSource>().Token;
        var monitorFactory = host.GetRequiredService<EventMonitorFactory>();
        var monitorBuilder = monitorFactory.When(eventKey);
        configMonitor?.Invoke(monitorBuilder);
        var monitor = monitorBuilder.Trigger(jobKey);
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
}
