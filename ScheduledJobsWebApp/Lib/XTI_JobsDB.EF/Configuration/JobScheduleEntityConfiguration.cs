namespace XTI_JobsDB.EF.Configuration;

internal sealed class JobScheduleEntityConfiguration : IEntityTypeConfiguration<JobScheduleEntity>
{
    public void Configure(EntityTypeBuilder<JobScheduleEntity> builder)
    {
        builder.HasKey(s => s.ID);
        builder.Property(s => s.ID).ValueGeneratedOnAdd();
        builder.HasIndex(s => s.JobDefinitionID).IsUnique();
        builder
            .HasOne<JobDefinitionEntity>()
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict)
            .HasForeignKey(s => s.JobDefinitionID);
        builder.ToTable("JobSchedules");
    }
}
