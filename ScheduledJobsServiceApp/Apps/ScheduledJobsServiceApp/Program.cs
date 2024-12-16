using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XTI_App.Api;
using XTI_HubAppClient.ServiceApp.Extensions;
using XTI_ScheduledJobsAppClient;
using XTI_ScheduledJobsServiceAppApi;

await XtiServiceAppHost.CreateDefault(ScheduledJobsAppKey.Value, args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddScheduledJobsAppApiServices();
        services.AddScoped<AppApiFactory, ScheduledJobsAppApiFactory>();
        services.AddScoped(sp => (ScheduledJobsAppApi)sp.GetRequiredService<IAppApi>());
        services.AddScheduledJobsAppClient();
    })
    .UseWindowsService()
    .Build()
    .RunAsync();