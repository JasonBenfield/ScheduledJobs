namespace XTI_Jobs.Abstractions;

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
    public JobSummaryModel(ExpandedTriggeredJobEntity source)
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

    public JobSummaryModel()
        : this
        (
            0,
            new JobKey(),
            JobTaskStatus.Values.NotSet,
            DateTimeOffset.MaxValue,
            DateTimeOffset.MaxValue,
            0
        )
    {
    }
}
