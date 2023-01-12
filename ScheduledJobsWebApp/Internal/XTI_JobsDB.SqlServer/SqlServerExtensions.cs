using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using XTI_Core;
using XTI_Core.Extensions;
using XTI_DB;
using XTI_JobsDB.EF;

namespace XTI_JobsDB.SqlServer;

public static class SqlServerExtensions
{
    public static void AddJobDbContextForSqlServer(this IServiceCollection services)
    {
        services.AddConfigurationOptions<DbOptions>(DbOptions.DB);
        services.AddDbContext<JobDbContext>((sp, options) =>
        {
            var xtiEnv = sp.GetRequiredService<XtiEnvironment>();
            var dbOptions = sp.GetRequiredService<DbOptions>();
            var connectionString = new XtiConnectionString
            (
                dbOptions, 
                new XtiDbName(xtiEnv.EnvironmentName, "Jobs")
            );
            options.UseSqlServer
            (
                connectionString.Value(),
                b => b.MigrationsAssembly("XTI_JobsDB.SqlServer")
            );
            if (xtiEnv.IsDevelopmentOrTest())
            {
                options.EnableSensitiveDataLogging();
            }
            else
            {
                options.EnableSensitiveDataLogging(false);
            }
        });
        services.AddScoped<DbAdmin<JobDbContext>>();
    }
}
