namespace XTI_Jobs.Abstractions;

public sealed record RegisteredJob(JobKey JobKey, TimeSpan Timeout, TimeSpan DeleteAfter, RegisteredJobTask[] Tasks);
