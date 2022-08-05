using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XTI_Core;
using XTI_Core.Extensions;
using XTI_DB;
using XTI_JobsDB.EF;
using XTI_JobsDB.SqlServer;

namespace XTI_JobsDbTool;

internal sealed class JobDbContextFactory : IDesignTimeDbContextFactory<JobDbContext>
{
    public JobDbContext CreateDbContext(string[] args)
    {
        var host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((hostingContext, config) =>
            {
                config.UseXtiConfiguration(hostingContext.HostingEnvironment, "", "", args);
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton(_ => new XtiEnvironment(hostContext.HostingEnvironment.EnvironmentName));
                services.AddConfigurationOptions<DbOptions>(DbOptions.DB);
                services.Configure<JobDbToolOptions>(hostContext.Configuration);
                services.AddJobDbContextForSqlServer();
            })
            .Build();
        var scope = host.Services.CreateScope();
        return scope.ServiceProvider.GetRequiredService<JobDbContext>();
    }
}