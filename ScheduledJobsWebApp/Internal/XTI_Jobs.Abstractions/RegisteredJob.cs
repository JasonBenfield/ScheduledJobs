namespace XTI_Jobs.Abstractions;

public sealed record RegisteredJob(JobKey JobKey, JobTaskKey[] TaskKeys);
