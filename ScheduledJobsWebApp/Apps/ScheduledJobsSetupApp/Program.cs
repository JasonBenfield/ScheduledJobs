using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ScheduledJobsSetupApp;
using XTI_App.Abstractions;
using XTI_App.Api;
using XTI_AppSetupApp.Extensions;
using XTI_Core;
using XTI_DB;
using XTI_JobsDB.EF;
using XTI_JobsDB.SqlServer;
using XTI_ScheduledJobsWebAppApi;

await XtiSetupAppHost.CreateDefault(ScheduledJobsInfo.AppKey, args)
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSingleton(_ => XtiEnvironment.Parse(hostContext.HostingEnvironment.EnvironmentName));
        services.AddSingleton(_ => AppVersionKey.Current);
        services.AddScoped<AppApiFactory, ScheduledJobsAppApiFactory>();
        services.AddScoped<IAppSetup, ScheduledJobsAppSetup>();
        services.AddJobDbContextForSqlServer();
        services.AddScoped<DbAdmin<JobDbContext>>();
    })
    .RunConsoleAsync();