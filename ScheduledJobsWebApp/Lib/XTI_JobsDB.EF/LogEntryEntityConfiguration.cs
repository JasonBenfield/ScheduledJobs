namespace XTI_JobsDB.EF;

internal sealed class LogEntryEntityConfiguration : IEntityTypeConfiguration<LogEntryEntity>
{
    public void Configure(EntityTypeBuilder<LogEntryEntity> builder)
    {
        builder.HasKey(e => e.ID);
        builder.Property(e => e.ID).ValueGeneratedOnAdd();
        builder.Property(e => e.Category).HasMaxLength(100);
        builder
            .HasOne<TriggeredJobTaskEntity>()
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict)
            .HasForeignKey(e => e.TaskID);
        builder.ToTable("LogEntries");
    }
}
