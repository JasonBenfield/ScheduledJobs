using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XTI_App.Api;
using XTI_Core;
using XTI_HubAppClient.ServiceApp.Extensions;
using XTI_Schedule;
using XTI_ScheduledJobsAppClient;
using XTI_ScheduledJobsServiceAppApi;

await XtiServiceAppHost.CreateDefault(ScheduledJobsInfo.AppKey, args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddScheduledJobsAppApiServices();
        services.AddScoped<AppApiFactory, ScheduledJobsAppApiFactory>();
        services.AddScoped(sp => (ScheduledJobsAppApi)sp.GetRequiredService<IAppApi>());
        services.AddSingleton<ScheduledJobsAppClientVersion>();
        services.AddScoped<ScheduledJobsAppClient>();
        services.AddAppAgenda
        (
            (sp, agenda) =>
            {
                agenda.AddScheduled<ScheduledJobsAppApi>
                (
                    (api, agendaItem) =>
                    {
                        agendaItem.Action(api.Jobs.PurgeJobsAndEvents.Path)
                            .Interval(TimeSpan.FromMinutes(15))
                            .AddSchedule
                            (
                                Schedule.EveryDay().At(TimeRange.From(1).ForOneHour())
                            );
                    }
                );
                agenda.AddScheduled<ScheduledJobsAppApi>
                (
                    (api, agendaItem) =>
                    {
                        agendaItem.Action(api.Jobs.TimeoutJobs.Path)
                            .Interval(TimeSpan.FromMinutes(15))
                            .AddSchedule
                            (
                                Schedule.EveryDay().At(TimeRange.AllDay())
                            );
                    }
                );
            }
        );
        services.AddThrottledLog<ScheduledJobsAppApi>
        (
            (api, throttled) => throttled.Throttle(api.Jobs.TimeoutJobs)
                .Requests().ForOneHour()
        );
    })
    .UseWindowsService()
    .Build()
    .RunAsync();