using System;
using XTI_Core;
using XTI_Core.Extensions;
using XTI_Core.Fakes;
using XTI_Schedule;

namespace XTI_ScheduledJobTests;

internal static class XtiTestHostExtensions
{
    public static async Task Register
    (
        this XtiHost host, Func<EventRegistrationBuilder, EventRegistrationBuilder1> configEvents,
        Func<JobRegistrationBuilder, JobRegistrationBuilder1> configJobs
    )
    {
        var events = host.GetRequiredService<EventRegistrationBuilder>();
        await configEvents(events).Build().Register();
        await host.RegisterJobs(configJobs);
    }

    public static Task RegisterJobs(this XtiHost host, Func<JobRegistrationBuilder, JobRegistrationBuilder1> configJobs)
    {
        var jobs = host.GetRequiredService<JobRegistrationBuilder>();
        return configJobs(jobs).Build().Register();
    }

    public static Task RegisterJobSchedule(this XtiHost host, JobKey jobKey, params Schedule[] schedules)
    {
        var registrationBuilder = host.GetRequiredService<JobScheduleRegistrationBuilder>();
        return registrationBuilder
            .Trigger(jobKey)
            .When(schedules)
            .Build()
            .Register();
    }

    public static Task<TriggeredJob[]> MonitorEvent(this XtiHost host, EventKey eventKey, JobKey jobKey, Action<EventMonitorBuilderFinal>? configMonitor = null)
    {
        var stoppingToken = host.GetRequiredService<CancellationTokenSource>().Token;
        var monitorBuilder = host.GetRequiredService<EventMonitorBuilder>();
        var jobActionFactory = host.GetRequiredService<DemoJobActionFactory>();
        var transformedEventData = host.GetRequiredService<DemoTransformedEventData>();
        var monitorBuilderFinal = monitorBuilder.When(eventKey)
            .Trigger(jobKey)
            .UseJobActionFactory(jobActionFactory)
            .TransformEventData(transformedEventData);
        configMonitor?.Invoke(monitorBuilderFinal);
        return monitorBuilderFinal.Build().Run(stoppingToken);
    }

    public static Task<TriggeredJob[]> MonitorScheduledJob(this XtiHost host, JobKey jobKey, Action<EventMonitorBuilderFinal>? configMonitor = null)
    {
        var stoppingToken = host.GetRequiredService<CancellationTokenSource>().Token;
        var monitorBuilder = host.GetRequiredService<EventMonitorBuilder>();
        var jobActionFactory = host.GetRequiredService<DemoJobActionFactory>();
        var monitorBuilderFinal = monitorBuilder.WhenScheduled(jobKey)
            .UseJobActionFactory(jobActionFactory);
        configMonitor?.Invoke(monitorBuilderFinal);
        return monitorBuilderFinal.Build().Run(stoppingToken);
    }

    public static Task<TriggeredJob[]> TriggerJobOnDemand(this XtiHost host, JobKey jobKey, params object[] data)
    {
        var stoppingToken = host.GetRequiredService<CancellationTokenSource>().Token;
        var onDemandBuilder = host.GetRequiredService<OnDemandJobBuilder>();
        var jobActionFactory = host.GetRequiredService<DemoJobActionFactory>();
        return onDemandBuilder
            .ForJob(jobKey)
            .WithData(data)
            .UseJobActionFactory(jobActionFactory)
            .Build()
            .RunUntilCompletion(stoppingToken);
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

    public static JobRegistrationBuilder1 BuildJobs(this JobRegistrationBuilder jobs) =>
        jobs
            .AddJob(DemoJobs.DoSomething.JobKey)
            .TimeoutAfter(TimeSpan.FromHours(1))
            .AddTasks
            (
                DemoJobs.DoSomething.GetAllTasks(),
                (t, j) => j.TimeoutAfter(TimeSpan.FromMinutes(5))
            );
}
