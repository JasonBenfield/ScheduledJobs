﻿namespace XTI_JobsDB.EF.Configuration;

internal sealed class TriggeredJobTaskEntityConfiguration : IEntityTypeConfiguration<TriggeredJobTaskEntity>
{
    public void Configure(EntityTypeBuilder<TriggeredJobTaskEntity> builder)
    {
        builder.HasKey(tjt => tjt.ID);
        builder.Property(tjt => tjt.ID).ValueGeneratedOnAdd();
        builder.HasIndex(tjt => new { tjt.TriggeredJobID, tjt.Sequence });
        builder
            .HasOne<TriggeredJobEntity>()
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict)
            .HasForeignKey(tjt => tjt.TriggeredJobID);
        builder
            .HasOne<JobTaskDefinitionEntity>()
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict)
            .HasForeignKey(tjt => tjt.TaskDefinitionID);
        builder.ToTable("TriggeredJobTasks");
    }
}
