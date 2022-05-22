﻿namespace XTI_JobsDB.EF;

internal sealed class JobDefinitionEntityConfiguration : IEntityTypeConfiguration<JobDefinitionEntity>
{
    public void Configure(EntityTypeBuilder<JobDefinitionEntity> builder)
    {
        builder.HasKey(jd => jd.ID);
        builder.Property(jd => jd.ID).ValueGeneratedOnAdd();
        builder.Property(jd => jd.JobKey).HasMaxLength(100);
        builder.HasIndex(jd => jd.JobKey).IsUnique();
        builder.ToTable("JobDefinitions");
    }
}