namespace XTI_Jobs.Abstractions;

public sealed record RegisteredJobTask(JobTaskKey TaskKey, TimeSpan Timeout);