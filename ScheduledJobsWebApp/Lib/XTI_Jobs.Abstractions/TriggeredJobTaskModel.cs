namespace XTI_Jobs.Abstractions;

public sealed record TriggeredJobTaskModel
(
    int ID, 
    JobTaskDefinitionModel TaskDefinition, 
    JobTaskStatus Status, 
    int Generation,
    DateTimeOffset TimeStarted,
    DateTimeOffset TimeEnded,
    string TaskData, 
    JobLogEntryModel[] LogEntries
);
