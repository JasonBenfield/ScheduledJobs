using XTI_Hub.Abstractions;

namespace XTI_Jobs.Abstractions;

public sealed record SourceLogEntryModel(int LogEntryID, AppLogEntryModel SourceLogEntry)
{
    public SourceLogEntryModel()
        : this(0, new AppLogEntryModel())
    {
    }
}
