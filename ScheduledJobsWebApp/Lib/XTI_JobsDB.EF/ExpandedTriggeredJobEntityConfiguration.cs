using XTI_Jobs.Abstractions;

namespace XTI_JobsDB.EF;

internal sealed class ExpandedTriggeredJobEntityConfiguration : IEntityTypeConfiguration<ExpandedTriggeredJobEntity>
{
    public void Configure(EntityTypeBuilder<ExpandedTriggeredJobEntity> builder)
    {
        builder.HasKey(j => j.JobID);
        builder.ToView("ExpandedTriggeredJobs");
    }
}
