using XTI_Core;
using XTI_Core.EF;

namespace XTI_JobsDB.EF;

public sealed class JobDbContext : DbContext
{
    private readonly UnitOfWork unitOfWork;

    public JobDbContext(DbContextOptions<JobDbContext> options)
        : base(options)
    {
        unitOfWork = new UnitOfWork(this);
        EventDefinitions = new EfDataRepository<EventDefinitionEntity>(this);
        EventNotifications = new EfDataRepository<EventNotificationEntity>(this);
        JobDefinitions = new EfDataRepository<JobDefinitionEntity>(this);
        JobTaskDefinitions = new EfDataRepository<JobTaskDefinitionEntity>(this);
        TriggeredJobs = new EfDataRepository<TriggeredJobEntity>(this);
        ExpandedTriggeredJobs = new EfDataRepository<ExpandedTriggeredJobEntity>(this);
        TriggeredJobTasks = new EfDataRepository<TriggeredJobTaskEntity>(this);
        HierarchicalTriggeredJobTasks = new EfDataRepository<HierarchicalTriggeredJobTaskEntity>(this);
        LogEntries = new EfDataRepository<LogEntryEntity>(this);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfiguration(new EventDefinitionEntityConfiguration());
        modelBuilder.ApplyConfiguration(new EventNotificationEntityConfiguration());
        modelBuilder.ApplyConfiguration(new JobDefinitionEntityConfiguration());
        modelBuilder.ApplyConfiguration(new JobTaskDefinitionEntityConfiguration());
        modelBuilder.ApplyConfiguration(new TriggeredJobEntityConfiguration());
        modelBuilder.ApplyConfiguration(new ExpandedTriggeredJobEntityConfiguration());
        modelBuilder.ApplyConfiguration(new TriggeredJobTaskEntityConfiguration());
        modelBuilder.ApplyConfiguration(new HierarchicalTriggeredJobTaskEntityConfiguration());
        modelBuilder.ApplyConfiguration(new LogEntryEntityConfiguration());
    }

    public DataRepository<EventDefinitionEntity> EventDefinitions { get; }

    public DataRepository<EventNotificationEntity> EventNotifications { get; }

    public DataRepository<JobDefinitionEntity> JobDefinitions { get; }

    public DataRepository<JobTaskDefinitionEntity> JobTaskDefinitions { get; }

    public DataRepository<TriggeredJobEntity> TriggeredJobs { get; }

    public DataRepository<ExpandedTriggeredJobEntity> ExpandedTriggeredJobs { get; }

    public DataRepository<TriggeredJobTaskEntity> TriggeredJobTasks { get; }

    public DataRepository<HierarchicalTriggeredJobTaskEntity> HierarchicalTriggeredJobTasks { get; }

    public DataRepository<LogEntryEntity> LogEntries { get; }

    public Task Transaction(Func<Task> action) => unitOfWork.Execute(action);
}
