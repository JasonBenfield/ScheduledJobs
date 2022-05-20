namespace XTI_JobsDB.EF;

internal sealed class JobTaskDefinitionEntityConfiguration : IEntityTypeConfiguration<JobTaskDefinitionEntity>
{
    public void Configure(EntityTypeBuilder<JobTaskDefinitionEntity> builder)
    {
        builder.HasKey(jd => jd.ID);
        builder.Property(jd => jd.ID).ValueGeneratedOnAdd();
        builder.Property(jd => jd.TaskKey).HasMaxLength(100);
        builder.HasIndex(jd => new { jd.JobDefinitionID, jd.TaskKey }).IsUnique();
        builder.ToTable("JobTaskDefinitions");
    }
}
