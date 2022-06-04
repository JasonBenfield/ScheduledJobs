namespace XTI_Jobs.Abstractions;

public sealed record RegisteredEvent
(
    EventKey EventKey, 
    bool CompareSourceKeyAndDataForDuplication, 
    DuplicateHandling DuplicateHandling,
    DateTimeOffset TimeToStartNotifications,
    TimeSpan ActiveFor,
    TimeSpan DeleteAfter
);
