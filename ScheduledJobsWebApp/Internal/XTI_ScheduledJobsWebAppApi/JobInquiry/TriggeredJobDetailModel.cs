namespace XTI_ScheduledJobsWebAppApi.JobInquiry;

public sealed record TriggeredJobDetailModel
(
    TriggeredJobModel Job,
    EventNotificationModel TriggeredBy,
    TriggeredJobTaskModel[] Tasks,
    SourceLogEntryModel[] SourceLogEntries
);
