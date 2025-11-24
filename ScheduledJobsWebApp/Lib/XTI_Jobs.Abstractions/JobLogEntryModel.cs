using XTI_Core;

namespace XTI_Jobs.Abstractions;

public sealed record class JobLogEntryModel
(
    int ID,
    AppEventSeverity Severity,
    DateTimeOffset TimeOccurred,
    string Category,
    string Message,
    string Details,
    string SourceEventKey
)
{
    public JobLogEntryModel()
        : this
        (
             ID: 0,
             Severity: AppEventSeverity.Values.NotSet,
             TimeOccurred: DateTimeOffset.MaxValue,
             Category: "",
             Message: "",
             Details: "",
             SourceEventKey: ""
        )
    {
    }
}
