using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XTI_Core;
using XTI_DB;
using XTI_JobsDB.EF;

namespace XTI_JobsDbTool;

internal sealed class HostedService : IHostedService
{
    private readonly IServiceProvider sp;

    public HostedService(IServiceProvider sp)
    {
        this.sp = sp;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = sp.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<JobDbContext>();
        var options = scope.ServiceProvider.GetRequiredService<JobDbToolOptions>();
        var xtiEnv = scope.ServiceProvider.GetRequiredService<XtiEnvironment>();
        try
        {
            if (options.Command == "reset")
            {
                if (!xtiEnv.IsTest() && !options.Force)
                {
                    throw new ArgumentException("Database reset can only be run for the test environment");
                }
                await scope.ServiceProvider.GetRequiredService<DbAdmin<JobDbContext>>().Reset();
            }
            else if (options.Command == "backup")
            {
                if (string.IsNullOrWhiteSpace(options.BackupFilePath))
                {
                    throw new ArgumentException("Backup file path is required for backup");
                }
                await scope.ServiceProvider.GetRequiredService<DbAdmin<JobDbContext>>().BackupTo(options.BackupFilePath);
            }
            else if (options.Command == "restore")
            {
                if (xtiEnv.IsProduction())
                {
                    throw new ArgumentException("Database restore cannot be run for the production environment");
                }
                if (string.IsNullOrWhiteSpace(options.BackupFilePath))
                {
                    throw new ArgumentException("Backup file path is required for restore");
                }
                await scope.ServiceProvider.GetRequiredService<DbAdmin<JobDbContext>>().RestoreFrom(options.BackupFilePath);
            }
            else if (options.Command == "update")
            {
                var mainDbContext = scope.ServiceProvider.GetRequiredService<JobDbContext>();
                await mainDbContext.Database.MigrateAsync();
            }
            else
            {
                throw new NotSupportedException($"Command '{options.Command}' is not supported");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            Environment.ExitCode = 999;
        }
        var lifetime = scope.ServiceProvider.GetRequiredService<IHostApplicationLifetime>();
        lifetime.StopApplication();
    }

    public Task StopAsync(CancellationToken cancellationToken)=> Task.CompletedTask;
}