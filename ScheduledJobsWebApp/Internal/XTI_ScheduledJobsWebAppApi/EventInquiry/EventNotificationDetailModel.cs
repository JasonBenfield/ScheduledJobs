namespace XTI_ScheduledJobsWebAppApi.EventInquiry;

public sealed record EventNotificationDetailModel
(
    EventNotificationModel Event,
    JobSummaryModel[] TriggeredJobs
);
