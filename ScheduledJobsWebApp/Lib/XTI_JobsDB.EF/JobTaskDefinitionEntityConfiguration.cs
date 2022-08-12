namespace XTI_JobsDB.EF;

internal sealed class JobTaskDefinitionEntityConfiguration : IEntityTypeConfiguration<JobTaskDefinitionEntity>
{
    public void Configure(EntityTypeBuilder<JobTaskDefinitionEntity> builder)
    {
        builder.HasKey(td => td.ID);
        builder.Property(td => td.ID).ValueGeneratedOnAdd();
        builder.Property(td => td.TaskKey).HasMaxLength(100);
        builder.Property(td => td.DisplayText).HasMaxLength(100);
        builder.Property(td => td.Timeout).HasConversion<string>();
        builder.HasIndex(td => new { td.JobDefinitionID, td.TaskKey }).IsUnique();
        builder.ToTable("JobTaskDefinitions");
    }
}
