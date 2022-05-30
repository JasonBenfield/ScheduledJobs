using Microsoft.Extensions.DependencyInjection;
using XTI_Core;
using XTI_Core.Extensions;
using XTI_Core.Fakes;
using XTI_JobsDB.EF;
using XTI_JobsDB.SqlServer;
using XTI_ScheduledJobsWebAppApi;

namespace XTI_ScheduledJobTests;

internal sealed class TestHost
{
    public static XtiHost CreateDefault()
    {
        var host = new XtiHostBuilder();
        host.Services.AddSingleton<FakeClock>();
        host.Services.AddSingleton<IClock>(sp => sp.GetRequiredService<FakeClock>());
        host.Services.AddJobDbContextForSqlServer();
        host.Services.AddScoped<IJobDb, EfJobDb>();
        host.Services.AddScoped<EventRegistration>();
        host.Services.AddScoped<JobRegistration>();
        host.Services.AddScoped<IncomingEventFactory>();
        host.Services.AddScoped<EventMonitorFactory>();
        host.Services.AddScoped<DemoJobActionFactory>();
        host.Services.AddScoped(sp => sp.GetRequiredService<DemoJobActionFactory>().Action01Context);
        host.Services.AddScoped(sp => sp.GetRequiredService<DemoJobActionFactory>().Action02Context);
        host.Services.AddScoped(sp => sp.GetRequiredService<DemoJobActionFactory>().ItemAction01Context);
        host.Services.AddScoped(sp => sp.GetRequiredService<DemoJobActionFactory>().ItemAction02Context);
        host.Services.AddScoped<IJobActionFactory>(sp => sp.GetRequiredService<DemoJobActionFactory>());
        host.Services.AddScheduledJobsAppApiServices();
        host.Services.AddScoped<ScheduledJobsAppApiFactory>();
        host.Services.AddScoped(sp => sp.GetRequiredService<ScheduledJobsAppApiFactory>().CreateForSuperUser());
        host.Services.AddSingleton<CancellationTokenSource>();
        return host.Build();
    }
}
