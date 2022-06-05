namespace XTI_ScheduledJobsWebAppApi.EventInquiry;

public sealed record EventSummaryModel(EventNotificationModel Event, int TriggeredJobCount);