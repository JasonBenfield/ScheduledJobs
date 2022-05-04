using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ScheduleJobSetupApp;
using XTI_App.Abstractions;
using XTI_App.Api;
using XTI_AppSetupApp.Extensions;
using XTI_ScheduleJobWebAppApi;

await XtiSetupAppHost.CreateDefault(ScheduleJobInfo.AppKey, args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSingleton(_ => AppVersionKey.Current);
        services.AddScoped<AppApiFactory, ScheduleJobAppApiFactory>();
        services.AddScoped<IAppSetup, ScheduleJobAppSetup>();
    })
    .RunConsoleAsync();