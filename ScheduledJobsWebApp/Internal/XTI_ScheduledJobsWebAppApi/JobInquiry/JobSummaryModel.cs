namespace XTI_ScheduledJobsWebAppApi.JobInquiry;

public sealed record JobSummaryModel
(
    int ID,
    JobKey JobKey,
    JobTaskStatus Status,
    DateTimeOffset TimeStarted,
    DateTimeOffset TimeEnded,
    int TaskCount
)
{
    internal JobSummaryModel(ExpandedTriggeredJobEntity source)
        : this
        (
            source.JobID,
            new JobKey(source.JobDisplayText),
            JobTaskStatus.Values.Value(source.JobStatus),
            source.TimeJobStarted,
            source.TimeJobEnded,
            source.TaskCount
        )
    {
    }
}
