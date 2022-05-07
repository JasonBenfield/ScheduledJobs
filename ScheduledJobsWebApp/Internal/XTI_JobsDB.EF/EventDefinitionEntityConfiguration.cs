namespace XTI_JobsDB.EF;

internal sealed class EventDefinitionEntityConfiguration : IEntityTypeConfiguration<EventDefinitionEntity>
{
    public void Configure(EntityTypeBuilder<EventDefinitionEntity> builder)
    {
        builder.HasKey(ed => ed.ID);
        builder.Property(ed => ed.ID).ValueGeneratedOnAdd();
        builder.Property(ed => ed.EventKey).HasMaxLength(100);
        builder.HasIndex(ed => ed.EventKey).IsUnique();
        builder.ToTable("EventDefinitions");
    }
}
