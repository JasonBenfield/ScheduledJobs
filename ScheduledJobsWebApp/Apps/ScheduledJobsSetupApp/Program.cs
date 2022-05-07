using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ScheduledJobsSetupApp;
using XTI_App.Abstractions;
using XTI_App.Api;
using XTI_AppSetupApp.Extensions;
using XTI_ScheduledJobsWebAppApi;

await XtiSetupAppHost.CreateDefault(ScheduledJobsInfo.AppKey, args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSingleton(_ => AppVersionKey.Current);
        services.AddScoped<AppApiFactory, ScheduledJobsAppApiFactory>();
        services.AddScoped<IAppSetup, ScheduledJobsAppSetup>();
    })
    .RunConsoleAsync();