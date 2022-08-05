using XTI_JobsDbTool;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XTI_Core;
using XTI_Core.Extensions;
using XTI_DB;
using XTI_Jobs;
using XTI_JobsDB.EF;
using XTI_JobsDB.SqlServer;

await Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, config) =>
    {
        config.UseXtiConfiguration(hostingContext.HostingEnvironment, "", "", args);
    })
    .ConfigureServices((hostContext, services) =>
    {
        services.AddSingleton(_ => new XtiEnvironment(hostContext.HostingEnvironment.EnvironmentName));
        services.AddConfigurationOptions<JobDbToolOptions>();
        services.AddConfigurationOptions<DbOptions>(DbOptions.DB);
        services.AddJobDbContextForSqlServer();
        services.AddScoped<DbAdmin<JobDbContext>>();
        services.AddHostedService<HostedService>();
    })
    .RunConsoleAsync();