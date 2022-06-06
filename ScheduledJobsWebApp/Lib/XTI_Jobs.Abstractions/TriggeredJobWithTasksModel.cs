namespace XTI_Jobs.Abstractions;

public sealed record TriggeredJobWithTasksModel(TriggeredJobModel Job, TriggeredJobTaskModel[] Tasks);
