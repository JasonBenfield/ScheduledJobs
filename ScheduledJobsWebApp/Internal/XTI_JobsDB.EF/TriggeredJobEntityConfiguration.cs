namespace XTI_JobsDB.EF;

internal sealed class TriggeredJobEntityConfiguration : IEntityTypeConfiguration<TriggeredJobEntity>
{
    public void Configure(EntityTypeBuilder<TriggeredJobEntity> builder)
    {
        builder.HasKey(tj => tj.ID);
        builder.Property(tj => tj.ID).ValueGeneratedOnAdd();
        builder
            .HasOne<JobDefinitionEntity>()
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict)
            .HasForeignKey(tj => tj.JobDefinitionID);
        builder
            .HasOne<EventNotificationEntity>()
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict)
            .HasForeignKey(tj => tj.EventNotificationID);
        builder.ToTable("TriggeredJobs");
    }
}
