namespace XTI_JobsDB.EF.Configuration;

internal sealed class JobDefinitionEntityConfiguration : IEntityTypeConfiguration<JobDefinitionEntity>
{
    public void Configure(EntityTypeBuilder<JobDefinitionEntity> builder)
    {
        builder.HasKey(jd => jd.ID);
        builder.Property(jd => jd.ID).ValueGeneratedOnAdd();
        builder.Property(jd => jd.JobKey).HasMaxLength(100);
        builder.HasIndex(jd => jd.JobKey).IsUnique();
        builder.Property(jd => jd.DisplayText).HasMaxLength(100);
        builder.Property(jd => jd.Timeout).HasConversion<string>();
        builder.Property(jd => jd.DeleteAfter).HasConversion<string>();
        builder.ToTable("JobDefinitions");
    }
}
