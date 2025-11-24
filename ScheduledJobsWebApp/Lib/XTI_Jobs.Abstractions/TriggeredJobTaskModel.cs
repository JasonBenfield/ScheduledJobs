namespace XTI_Jobs.Abstractions;

public sealed record TriggeredJobTaskModel
(
    int ID,
    JobTaskDefinitionModel TaskDefinition,
    JobTaskStatus Status,
    int Generation,
    DateTimeOffset TimeStarted,
    DateTimeOffset TimeEnded,
    string TaskData,
    JobLogEntryModel[] LogEntries
)
{
    public TriggeredJobTaskModel()
        : this
        (
             ID: 0,
             TaskDefinition: new(),
             Status: JobTaskStatus.Values.NotSet,
             Generation: 0,
             TimeStarted: DateTimeOffset.MaxValue,
             TimeEnded: DateTimeOffset.MaxValue,
             TaskData: "",
             LogEntries: []
        )
    {
    }
}
