namespace XTI_JobsDB.EF.Configuration;

internal sealed class EventNotificationEntityConfiguration : IEntityTypeConfiguration<EventNotificationEntity>
{
    public void Configure(EntityTypeBuilder<EventNotificationEntity> builder)
    {
        builder.HasKey(en => en.ID);
        builder.Property(en => en.ID).ValueGeneratedOnAdd();
        builder
            .HasOne<EventDefinitionEntity>()
            .WithMany()
            .OnDelete(DeleteBehavior.Restrict)
            .HasForeignKey(en => en.EventDefinitionID);
        builder.ToTable("EventNotifications");
    }
}
