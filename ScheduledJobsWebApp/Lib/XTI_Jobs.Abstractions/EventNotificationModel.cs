namespace XTI_Jobs.Abstractions;

public sealed record EventNotificationModel
(
    int ID, 
    EventDefinitionModel Definition, 
    string SourceKey,
    string SourceData,
    DateTimeOffset TimeAdded,
    DateTimeOffset TimeActive, 
    DateTimeOffset TimeInactive
);