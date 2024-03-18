namespace XTI_Jobs.Abstractions;

public sealed record EventNotificationDetailModel
(
    EventNotificationModel Event,
    JobSummaryModel[] TriggeredJobs
);
