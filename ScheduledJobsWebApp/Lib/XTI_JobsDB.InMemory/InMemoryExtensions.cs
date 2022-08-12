using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using XTI_JobsDB.EF;

namespace XTI_JobsDB.InMemory;

public static class InMemoryExtensions
{
    public static void AddJobDbContextForInMemory(this IServiceCollection services)
    {
        services.AddDbContextFactory<JobDbContext>(options =>
        {
            options
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .EnableSensitiveDataLogging();
        });
    }
}
