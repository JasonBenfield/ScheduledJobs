using XTI_Core;
using XTI_Core.EF;

namespace XTI_JobsDB.EF;

public sealed class JobDbContext : DbContext
{
    public JobDbContext(DbContextOptions<JobDbContext> options)
        : base(options)
    {
        EventDefinitions = new EfDataRepository<EventDefinitionEntity>(this);
        EventNotifications = new EfDataRepository<EventNotificationEntity>(this);
        JobDefinitions = new EfDataRepository<JobDefinitionEntity>(this);
        TriggeredJobs = new EfDataRepository<TriggeredJobEntity>(this);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new EventDefinitionEntityConfiguration());
        modelBuilder.ApplyConfiguration(new EventNotificationEntityConfiguration());
        modelBuilder.ApplyConfiguration(new JobDefinitionEntityConfiguration());
        modelBuilder.ApplyConfiguration(new TriggeredJobEntityConfiguration());
    }

    public DataRepository<EventDefinitionEntity> EventDefinitions { get; }

    public DataRepository<EventNotificationEntity> EventNotifications { get; }

    public DataRepository<JobDefinitionEntity> JobDefinitions { get; }

    public DataRepository<TriggeredJobEntity> TriggeredJobs { get; }
}
