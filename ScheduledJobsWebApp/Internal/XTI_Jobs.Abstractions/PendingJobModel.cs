namespace XTI_Jobs.Abstractions;

public sealed record PendingJobModel(TriggeredJobModel Job, string SourceData);
