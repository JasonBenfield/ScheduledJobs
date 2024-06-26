﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Hosting.Internal;
using XTI_App.Secrets;
using XTI_Core;
using XTI_Core.Extensions;
using XTI_Core.Fakes;
using XTI_HubAppClient.Extensions;
using XTI_JobsDB.EF;
using XTI_JobsDB.SqlServer;
using XTI_ScheduledJobsAppClient;
using XTI_ScheduledJobsWebAppApi;
using XTI_Secrets.Extensions;

namespace XTI_ScheduledJobIntegrationTests;

internal sealed class TestHost
{
    public static XtiHost CreateDefault() => CreateDefault(XtiEnvironment.Test);

    public static XtiHost CreateDefault(XtiEnvironment xtiEnv)
    {
        Environment.SetEnvironmentVariable("DOTNET_ENVIRONMENT", xtiEnv.EnvironmentName);
        var host = new XtiHostBuilder(xtiEnv);
        host.Services.AddMemoryCache();
        host.Services.AddSingleton<IHostEnvironment>(new HostingEnvironment
        {
            EnvironmentName = xtiEnv.EnvironmentName
        });
        host.Services.AddSingleton<FakeClock>();
        host.Services.AddSingleton<IClock>(sp => sp.GetRequiredService<FakeClock>());
        host.Services.AddJobDbContextForSqlServer();
        host.Services.AddHubClientServices();
        host.Services.AddSingleton<ScheduledJobsAppClientVersion>();
        host.Services.AddScoped<ScheduledJobsAppClient>();
        host.Services.AddScoped<IJobDb, EfJobDb>();
        host.Services.AddScoped<EventRegistrationBuilder>();
        host.Services.AddScoped<JobRegistrationBuilder>();
        host.Services.AddScoped<IncomingEventFactory>();
        host.Services.AddScoped<EventMonitorBuilder>();
        host.Services.AddScoped<JobScheduleRegistrationBuilder>();
        host.Services.AddScoped<OnDemandJobBuilder>();
        host.Services.AddScoped<DemoJobActionFactory>();
        host.Services.AddScoped<DemoTransformedEventData>();
        host.Services.AddScoped(sp => sp.GetRequiredService<DemoJobActionFactory>().Action01Context);
        host.Services.AddScoped(sp => sp.GetRequiredService<DemoJobActionFactory>().Action02Context);
        host.Services.AddScoped(sp => sp.GetRequiredService<DemoJobActionFactory>().ItemAction01Context);
        host.Services.AddScoped(sp => sp.GetRequiredService<DemoJobActionFactory>().ItemAction02Context);
        host.Services.AddScoped<IJobActionFactory>(sp => sp.GetRequiredService<DemoJobActionFactory>());
        host.Services.AddScheduledJobsAppApiServices();
        host.Services.AddScoped<ScheduledJobsAppApiFactory>();
        host.Services.AddScoped(sp => sp.GetRequiredService<ScheduledJobsAppApiFactory>().CreateForSuperUser());
        host.Services.AddSingleton<CancellationTokenSource>();
        host.Services.AddFileSecretCredentials(xtiEnv);
        host.Services.AddInstallationUserXtiToken();
        host.Services.AddSingleton<AdminUserCredentials>();
        host.Services.AddScoped<AdminUserXtiToken>();
        host.Services.AddXtiTokenAccessorFactory
        (
            (sp, accessor) =>
            {
                accessor.AddToken(() => sp.GetRequiredService<InstallationUserXtiToken>());
                accessor.AddToken(() => sp.GetRequiredService<AdminUserXtiToken>());
                accessor.UseDefaultToken<InstallationUserXtiToken>();
            }
        );
        return host.Build();
    }
}
