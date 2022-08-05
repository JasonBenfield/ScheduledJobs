﻿namespace XTI_Jobs;

public sealed class EventBuilder
{
    private readonly EventKey eventKey;
    private TimeSpan deleteAfter = TimeSpan.FromDays(365);
    private TimeSpan activeFor = TimeSpan.MaxValue;

    internal EventBuilder(EventKey eventKey)
    {
        this.eventKey = eventKey;
    }

    private DateTimeOffset TimeToStartNotification { get; set; } = DateTimeOffset.MinValue;
    internal bool CompareSourceKeyAndDataForDuplication { get; set; } = true;
    internal DuplicateHandling DuplicateHandling { get; set; } = DuplicateHandling.Values.Ignore;

    public EventDuplicationBuilder Ignore() => Duplicates(DuplicateHandling.Values.Ignore);

    public EventDuplicationBuilder KeepNewest() => Duplicates(DuplicateHandling.Values.KeepNewest);

    public EventDuplicationBuilder KeepOldest() => Duplicates(DuplicateHandling.Values.KeepOldest);

    public EventDuplicationBuilder KeepAll() => Duplicates(DuplicateHandling.Values.KeepAll);

    private EventDuplicationBuilder Duplicates(DuplicateHandling duplicateHandling)
    {
        DuplicateHandling = duplicateHandling;
        return new EventDuplicationBuilder(this);
    }

    public EventBuilder StartNotifying(DateTimeOffset timeToStartNotifications)
    {
        TimeToStartNotification = timeToStartNotifications;
        return this;
    }

    public EventBuilder ActiveFor(TimeSpan activeFor)
    {
        this.activeFor = activeFor;
        return this;
    }

    public EventBuilder DeleteAfter(TimeSpan deleteAfter)
    {
        this.deleteAfter = deleteAfter;
        return this;
    }

    internal RegisteredEvent Build() => 
        new RegisteredEvent
        (
            eventKey, 
            CompareSourceKeyAndDataForDuplication, 
            DuplicateHandling, 
            TimeToStartNotification,
            activeFor,
            deleteAfter
        );
}