namespace XTI_JobsDB.EF.Configuration;

internal sealed class HierarchicalTriggeredJobTaskEntityConfiguration : IEntityTypeConfiguration<HierarchicalTriggeredJobTaskEntity>
{
    public void Configure(EntityTypeBuilder<HierarchicalTriggeredJobTaskEntity> builder)
    {
        builder.HasKey(htjt => htjt.ID);
        builder.Property(htjt => htjt.ID).ValueGeneratedOnAdd();
        builder.HasIndex(htjt => new { htjt.ParentTaskID, htjt.ChildTaskID });
        builder
            .HasOne<TriggeredJobTaskEntity>()
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict)
            .HasForeignKey(htjt => htjt.ParentTaskID);
        builder
            .HasOne<TriggeredJobTaskEntity>()
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict)
            .HasForeignKey(htjt => htjt.ChildTaskID);
        builder.ToTable("HierarchicalTriggeredJobTasks");
    }
}
