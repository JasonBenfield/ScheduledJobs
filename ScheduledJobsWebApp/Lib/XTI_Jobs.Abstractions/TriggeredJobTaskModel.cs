namespace XTI_Jobs.Abstractions;

public sealed record TriggeredJobTaskModel
(
    int ID, 
    JobTaskDefinitionModel TaskDefinition, 
    JobTaskStatus Status, 
    DateTimeOffset TimeStarted,
    DateTimeOffset TimeEnded,
    string TaskData, 
    LogEntryModel[] LogEntries
);
