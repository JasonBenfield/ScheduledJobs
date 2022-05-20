namespace XTI_Jobs.Abstractions;

public sealed record TriggeredJobDetailModel(TriggeredJobModel Job, TriggeredJobTaskModel[] Tasks);
