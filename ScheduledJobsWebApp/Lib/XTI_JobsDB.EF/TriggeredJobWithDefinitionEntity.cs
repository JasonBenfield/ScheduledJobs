namespace XTI_JobsDB.EF;

internal sealed record TriggeredJobWithDefinitionEntity(TriggeredJobEntity Job, JobDefinitionEntity Definition);
