namespace XTI_Jobs.Abstractions;

public sealed record EventSummaryModel(EventNotificationModel Event, int TriggeredJobCount);