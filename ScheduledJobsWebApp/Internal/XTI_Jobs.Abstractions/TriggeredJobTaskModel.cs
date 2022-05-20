namespace XTI_Jobs.Abstractions;

public sealed record TriggeredJobTaskModel(int ID, JobTaskDefinitionModel TaskDefinition, JobTaskStatus Status, string TaskData, LogEntryModel[] LogEntries);
